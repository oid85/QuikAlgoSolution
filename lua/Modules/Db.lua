-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Math.lua")
dofile(getScriptPath().."\\Modules\\String.lua")
dofile(getScriptPath().."\\Modules\\Time.lua")

-- данные для подключения к БД
g_dbName   = "algo"
g_user     = "root"
g_password = "1"
g_host     = "localhost"

-- подключение библиотек
require "luasql.mysql"

local environment = nil
local connection = nil

--- описание: открытие соединения с БД
--- параметр: параметры отсутствуют
--- результат: открытие соединения с БД (тип результата: действие)
function OpenDbConnection()
    -- вызываем основную функцию из библиотеки
    environment = assert(luasql.mysql())

    -- соединяемся с базой данных
	connection = assert(environment:connect(g_dbName, g_user, g_password, g_host))
end

--- описание: закрытие соединения с БД
--- параметр: параметры отсутствуют
--- результат: закрытие соединения с БД (тип результата: действие)
function CloseDbConnection()
    -- закрытие соединения с базой данных
    connection : close()
    connection = nil

    -- закрытие вызова библиотеки
    environment:close()
    environment = nil
end

--- описание: тест соединения с БД
--- параметр: параметры отсутствуют
--- результат: тест соединения с БД (тип результата: bool)
function TestDbConnection()
	OpenDbConnection()

    if (connection == nil) then
        CloseDbConnection()
        return false
    end

    CloseDbConnection()

	return true
end

--- описание: выполнить запрос select
--- параметр: sql - текст запроса (тип параметра: string)
--- результат: результат запроса (тип результата: table)
function ExecuteReader(sql)
	return assert(connection:execute(sql))
end

--- описание: выполнить запрос insert или update
--- параметр: sql - текст запроса (тип параметра: string)
--- результат: результат запроса (тип результата: действие)
function ExecuteQuery(sql)
	assert(connection:execute(sql))
end

--- описание: количество строк в таблице БД
--- параметр: tableName - имя таблицы БД (тип параметра: string)
--- результат: количество строк в таблице БД (тип результата: number)
function CountRowsOfDbTable(tableName)
	OpenDbConnection()

	-- создаём и открываем курсор
    local cursor = ExecuteReader(string.format([[SELECT id FROM %s]], tableName))

	-- определение количества строк в запросе
    local count = cursor:numrows()

    cursor = nil

	CloseDbConnection()

    return count
end

--- описание: сохранить свечи в БД
--- параметр: tableName - имя таблицы БД (тип параметра: string)
--- параметр: candles - свечи (тип результата: table)
--- параметр: timeframe - таймфрейм (тип результата: number)
--- результат: сохранение свечей в БД (тип результата: number)
function AddCandlesToDb(tableName, candles, timeframe)
	OpenDbConnection()

	-- Добавить проверку на длину массива candles

    local numberForUpdate = 5 -- количество свечек с конца, которые всегда перезаписываем

    for i = 1, #candles do
        local date = StringDate(candles[i].datetime)
        local time = StringTime(candles[i].datetime)
        local open = tostring(candles[i].open)
        local close = tostring(candles[i].close)
        local high = tostring(candles[i].high)
        local low = tostring(candles[i].low)
        local volume = tostring(candles[i].volume)
		local tf = tostring(timeframe)
        local modifidedDateTime = StringDateTimeFormat(os.date("*t", os.time()))

        -- читаем из БД одну(!) запись
        local cursor = ExecuteReader(string.format([[SELECT id FROM %s WHERE date = '%s' AND time = '%s' LIMIT 1]], tableName, date, time))

        -- определение количества строк в запросе
        local count = cursor:numrows()

        cursor = nil

        if (i <= #candles - numberForUpdate) then
            if (count == 0) then -- если свечу ранее не записывали
                ExecuteQuery(string.format([[INSERT INTO %s (date, time, open, close, high, low, volume, timeframe, modifided_datetime)
                                             VALUES ('%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s')]],
                                             tableName,
                                             date, time, open, close, high, low, volume, tf, modifidedDateTime))
            else -- если свечу ранее уже записывали
                -- ...
            end
        else
            if (count == 0) then -- если свечу ранее не записывали
                ExecuteQuery(string.format([[INSERT INTO %s (date, time, open, close, high, low, volume, timeframe, modifided_datetime)
                                             VALUES ('%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s')]],
                                             tableName,
                                             date, time, open, close, high, low, volume, tf, modifidedDateTime))
            else -- если свечу ранее уже записывали
                ExecuteQuery(string.format([[UPDATE %s
                                             SET open = '%s', close = '%s', high = '%s', low = '%s', volume = '%s', timeframe = '%s', modifided_datetime = '%s'
                                             WHERE date = '%s' AND time = '%s']],
                                             tableName,
                                             open, close, high, low, volume, tf, modifidedDateTime,
                                             date, time))
            end
        end
    end

	CloseDbConnection()
end

--- описание: сохранить состояние счета в БД
--- параметр: money - состояние счета (тип параметра: number)
--- результат: сохранение состояния счета в БД (тип результата: действие)
function AddMoneyFortsToDb(money)	
	OpenDbConnection()
	
	local tableName = "money_forts"
	local hours = 12 -- пишем в БД только если записывали больше hours часов назад
	local date
	local time
	local currentDate = NumberCurrentDate()
	local currentTime = NumberCurrentTime()

	-- читаем из БД одну(!) последнюю запись
	local cursor = ExecuteReader(string.format([[SELECT * FROM %s ORDER BY Id DESC LIMIT 1]], tableName))
	
	local row = cursor: fetch ({}, "a")
	
	-- определение количества строк в запросе
	local count = cursor:numrows()	
	
	if (count == 0) then
		ExecuteQuery(string.format([[INSERT INTO %s (date, time, money)
									 VALUES ('%s', '%s', '%s')]],
									 tableName,
									 tostring(currentDate), tostring(currentTime), tostring(money)))	
		return
	end
	
	while (row) do
		date = tonumber(row.date)
		time = tonumber(row.time)
		row = cursor: fetch (row, "a")
	end	
	
	local t1 = date * 1000000 + time
	local t2 = currentDate * 1000000 + currentTime
	
	if (t2 - t1 > hours * 60 * 60) then
		ExecuteQuery(string.format([[INSERT INTO %s (date, time, money)
									 VALUES ('%s', '%s', '%s')]],
									 tableName,
									 tostring(currentDate), tostring(currentTime), tostring(money)))	
	end
	
	CloseDbConnection()
end

--- описание: сохранить свечу в БД
--- параметр: tableName - имя таблицы БД (тип параметра: string)
--- параметр: candle - свеча (тип результата: table)
--- результат: сохранение свечи в БД (тип результата: действие)
function AddCandleToDb(tableName, candle)
    local columns = { "date", "time", "open", "close", "high", "low", "volume", "modifided_datetime" }
    local values =
    {
        StringDate(candle.datetime), StringTime(candle.datetime),
        tostring(candle.open), tostring(candle.close), tostring(candle.high), tostring(candle.low),
        tostring(candle.volume),
        StringDateTimeFormat(os.date("*t", os.time()))
    }

    -- заносим параметры свечи в БД
    AddRowToDb(tableName, columns, values)
end

--- описание: сохранить лог-сообщение в БД
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: сохранение лог-сообщения в БД (тип результата: действие)
function AddLogMessageToDb(message)
    local tableName = "logs"
	
	-- колонки
    local columns = 
	{ 
		"datetime", 
		"type", 
		"source", 
		"text", 
		"details", 
		"guid" 
	}
	
	-- значения
    local values = 
	{ 
		tostring(message.datetime), 
		tostring(message.type), 
		tostring(message.source), 
		tostring(message.text), 
		tostring(message.details), 
		tostring(message.guid) 
	}

    -- записываем лог-сообщение в БД
    AddRowToDb(tableName, columns, values)
end

--- описание: сохранить сделку из Quik в БД
--- параметр: quikTrade - свеча (тип результата: table)
--- результат: сохранение сделки из Quik в БД (тип результата: действие)
function AddTradeToDb(quikTrade)
	local logSource = "Db.AddTrade(quikTrade)"
    
    -- если сделка активна или не исполнена, то выходим
    if (BitIsTrue(quikTrade.flags, 0) or BitIsTrue(quikTrade.flags, 1)) then 
        return 
    end 
    
    OpenDbConnection()

	-- читаем данные, которые будем заносить в базу данных
    local strategyId    = "-" -- идентификатор стратегии
    local tradeNum      = "-" -- номер сделки
	local orderNum      = "-" -- номер заявки
	local brokerRef     = "-" -- комментарий
	local account       = "-" -- торговый счет
	local price         = "-" -- цена
	local qty           = "-" -- количество бумаг в сделке в лотах
	local classCode     = "-" -- код класса
	local secCode       = "-" -- код бумаги
	local tradeDatetime = "-" -- время сделки
	local tradeType     = "-" -- тип сделки

	-- проверка значений на nil
	if (quikTrade.trade_num ~= nil) then tradeNum = tostring(quikTrade.trade_num) end
	if (quikTrade.order_num ~= nil) then orderNum = tostring(quikTrade.order_num) end
	if (quikTrade.brokerref ~= nil) then brokerRef = tostring(quikTrade.brokerref) end
	if (quikTrade.account ~= nil) then account = tostring(quikTrade.account) end
    if (quikTrade.price ~= nil) then price = tostring(quikTrade.price) end
    
    if (quikTrade.flags ~= nil) then 
        if (BitIsTrue(flags, 2)) then tradeType = "S" end
        tradeType = "B"
    end
    
	if (quikTrade.qty ~= nil) then
		local quantity = 0
		if (tradeType == "B") then quantity = math.abs(tonumber(quikTrade.qty)) end
		if (tradeType == "S") then quantity = -1 * math.abs(tonumber(quikTrade.qty)) end
		qty = tostring(quantity)
    end
    
	if (quikTrade.class_code ~= nil) then classCode = tostring(quikTrade.class_code) end
	if (quikTrade.sec_code ~= nil) then secCode = tostring(quikTrade.sec_code) end
	if (quikTrade.datetime ~= nil) then tradeDatetime = StringDate(quikTrade.datetime)..StringTime(quikTrade.datetime) end

	-- проверяем, записывали ли ранее эту сделку
	-- создаём и открываем курсор
	local cursor = assert(connection: execute(string.format([[SELECT trade_num, order_num FROM trades WHERE trade_num = '%s' AND order_num = '%s']], quikTrade.trade_num, quikTrade.order_num)))

	-- определение количества строк в запросе
	local countRows = cursor: numrows()
	if (countRows == 0) then
		-- заносим параметры сделки в БД
		local res = assert(connection: execute(string.format([[INSERT INTO trades (strategy_name, strategy_id, trade_num, order_num, brokerref, account, price, qty, class_code, sec_code, datetime, trade_type) 
															   VALUES ('%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s')]],
																			 g_strategyName, tostring(g_strategyId), tradeNum, orderNum, brokerRef, account, price, qty, classCode, secCode, tradeDatetime, tradeType)))
	end
    cursor: close() -- закрытие курсора
    
	CloseDbConnection()

	LogDebug(logSource, g_logMessages["0005"]..string.format([[ ('%s';'%s';'%s';'%s';'%s';'%s';'%s';'%s';'%s';'%s';'%s';'%s')]],
															  g_strategyName, tostring(g_strategyId), tradeNum, orderNum, brokerRef, 
															  account, price, qty, classCode, secCode, tradeDatetime, tradeType), "{A87C855D-1833-42C8-A2D1-344A432DBB2C}")
end

--- описание: добавить строку в таблицу БД
--- параметр: tableName - имя таблицы (тип параметра: string)
--- параметр: columns - перечень колонок (тип параметра: table)
--- параметр: values - перечень значений (тип параметра: table)
--- результат: добавление строки в таблицу БД (тип результата: действие)
function AddRowToDb(tableName, columns, values)
	OpenDbConnection()
    ExecuteQuery(string.format([[INSERT INTO %s (%s) VALUES (%s)]], tableName, StringJoin(columns, ",", ""), StringJoin(values, ",", "'")))
	CloseDbConnection()
end

--- описание: обновить строку аккаунта в БД
--- параметр: account - имя аккаунта (тип параметра: string)
--- параметр: money - размер счета (тип параметра: number)
--- результат: обновление строки аккаунта в БД (тип результата: действие)
function UpdateAccountInDb(account, money)
	OpenDbConnection()

    local modifidedDateTime = StringDateTimeFormat(os.date("*t", os.time()))

    -- читаем из БД одну(!) запись
    local cursor = ExecuteReader(string.format([[SELECT id FROM accounts WHERE account = '%s' LIMIT 1]], account))

    -- определение количества строк в запросе
    local count = cursor:numrows()

    cursor = nil

    if (count == 0) then -- если аккаунт ранее не записывали	
        ExecuteQuery(string.format([[INSERT INTO accounts (account, money, modifided_datetime) VALUES ('%s', '%s', '%s')]], 
                                     tostring(account), tostring(money), modifidedDateTime))
    else -- если аккаунт ранее уже записывали
        ExecuteQuery(string.format([[UPDATE accounts SET money = '%s', modifided_datetime = '%s' WHERE account = '%s']],
                                     tostring(money), modifidedDateTime, tostring(account)))
    end

    CloseDbConnection()
end

--- описание: обновить строку гарантийного обеспечения в БД
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- параметр:  gaurant - размер гарантийного обеспечения (тип параметра: number)
--- результат: обновление строки гарантийного обеспечения в БД (тип результата: действие)
function UpdateGaurantInDb(classCode, securityCode, gaurant)
	OpenDbConnection()

    local modifidedDateTime = StringDateTimeFormat(os.date("*t", os.time()))

    -- читаем из БД одну(!) запись
    local cursor = ExecuteReader(string.format([[SELECT id FROM securities WHERE class_code = '%s' AND security_code = '%s' LIMIT 1]], classCode, securityCode))

    -- определение количества строк в запросе
    local count = cursor:numrows()

    cursor = nil

    if (count == 0) then -- если аккаунт ранее не записывали
        ExecuteQuery(string.format([[INSERT INTO securities (class_code, security_code, gaurant, modifided_datetime) 
									 VALUES ('%s', '%s', '%s', '%s')]],
                                     tostring(classCode), tostring(securityCode), tostring(gaurant), modifidedDateTime))
    else -- если аккаунт ранее уже записывали
        ExecuteQuery(string.format([[UPDATE securities SET gaurant = '%s', modifided_datetime = '%s' WHERE class_code = '%s' AND security_code = '%s']],
                                     tostring(gaurant), modifidedDateTime, tostring(classCode), tostring(securityCode)))
    end

    CloseDbConnection()
end

--- описание:  размер позиции
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: размер позиции (тип результата: number)
function TargetPosition(classCode, securityCode)
	OpenDbConnection()

	local currentPosition = 0
	local cursor = ExecuteReader(string.format([[SELECT * FROM strategies WHERE class_code = '%s' AND security_code = '%s']], classCode, securityCode))
	local row = cursor: fetch ({}, "a")
	while (row) do
		currentPosition = currentPosition + tonumber(row.order_quantity) -- суммируем общий результат по всем сделкам
		row = cursor: fetch (row, "a")
	end
	
	CloseDbConnection()

	return currentPosition
end