-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Db.lua")
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")
dofile(getScriptPath().."\\Modules\\Table.lua")
dofile(getScriptPath().."\\Modules\\Time.lua")

--- описание:  пауза в работе и вывод сообщения об ошибке в таблицу мониторинга
--- параметр:  параметры отсутствуют
--- результат: результат результат проверки (0 или 1) (тип результата: number)
function WaitingProcessErrors()
    local logSource = "Check.WaitingProcessErrors()"
	if (g_wait > 0) then 
		g_wait = g_wait - 1 
        UpdateTableValue("Message", g_messageText)
		sleep(1000)
		return
	end	
end

--- описание:  не торговое время
--- параметр:  параметры отсутствуют
--- результат: результат результат проверки (0 или 1) (тип результата: number)
function NoTradingTime()
    local logSource = "Check.IsTradingTime()"
    if (g_startTime == nil) then 
        g_messageText = g_messages["3XP2"]
        LogError({  
            source = logSource, 
            text = g_messageText, 
            details = "g_startTime", 
            guid = "9298cc86-235b-4a58-8167-0ef617e36e10" })
        return 1 
    end	
    if (g_endTime == nil) then 
        g_messageText = g_messages["3XP2"]
        LogError({ 
            source = logSource, 
            text = g_messageText, 
            details = "g_endTime", 
            guid = "14d2ce2e-1f36-4caa-a133-53e9128dd6e5" })
        return 2 
    end	    
    if (NumberCurrentTime() < g_startTime) then 
        g_messageText = g_messages["I93O"]
        LogTrace({ 
            source = logSource, 
            text = g_messageText, 
            details = "g_endTime", 
            guid = "820842be-f37b-4a4a-85b7-a9a820f45131" })        
        return 3 
    end	
    if (NumberCurrentTime() > g_endTime) then 
        g_messageText = g_messages["I93O"]
        LogTrace({ 
            source = logSource, 
            text = g_messageText, 
            details = "g_endTime", 
            guid = "fccb8794-cae4-4d53-a4dc-a66775714ae3" })        
        return 4 
    end	    
	return 0
end

--- описание:  БД не доступна
--- параметр:  параметры отсутствуют
--- результат: результат результат проверки (0 или 1) (тип результата: number)
function DbNotAvailable()
    local logSource = "Check.DbNotAvailable()"
	if (TestDbConnection() == false) then 
		g_messageText = g_messages["2HB7"] 
		g_wait = g_waitLimit
		LogError({ 
            source = logSource, 
            text = g_messageText, 
            guid = "2c8f7b11-85bb-455f-8335-126f887e2204" })
		return 1
    end	
    return 0
end

--- описание:  сервер Quik не доступен
--- параметр:  параметры отсутствуют
--- результат: результат результат проверки (0 или 1) (тип результата: number)
function ServerNotAvailable()
    local logSource = "Check.ServerNotAvailable()"
	local serverTime = getInfoParam("SERVERTIME")
	if (serverTime == nil or serverTime == "") then 
		g_messageText = g_messages["OL2F"]
        g_wait = g_waitLimit
		LogError({ 
            source = logSource, 
            text = g_messageText, 
            guid = "f533fea9-6a92-42f8-b2df-bb4313733923" })        
		return 1
	end	
    UpdateTableValue("ServerTime", tostring(serverTime))
    return 0
end

--- описание:  свечи инструмента не доступны
--- параметр:  identPrice - идентификатор графика цены (тип параметра: string)
--- результат: результат результат проверки (0 или 1) (тип результата: number)
function CandlesNotAvailable(identPrice)
    local logSource = "Check.CandlesNotAvailable(identPrice)"
	local countCandles = getNumCandles(identPrice) -- количество свечей на графике цены
	if (countCandles == nil) then 
		g_messageText = g_messages["59X5"]..string.format([[ Details: dentPrice = %s]], tostring(identPrice))
		g_wait = g_waitLimit 
		LogError({ 
            source = logSource, 
            text = g_messageText, 
            guid = "0c7d31cb-42a8-4706-9ca0-f4a851136ac0" })
		return 1
    end
    return 0
end