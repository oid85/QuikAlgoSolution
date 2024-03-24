-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Check.lua")
dofile(getScriptPath().."\\Modules\\Candle.lua")
dofile(getScriptPath().."\\Modules\\Math.lua")

--===============================================НАСТРОЙКИ=======================================================
-- расположение таблицы мониторинга
g_left   = 0   -- отступ слева
g_top    = 200   -- отступ сверху
g_width  = 400 -- ширина таблицы мониторинга
g_height = 200 -- высота таблицы мониторинга

g_interval = 5000 -- интервал опроса терминала в миллисекундах

-- начало и конец торговой сессии
g_startTime = 100100 -- 10:01:00
g_endTime   = 234600 -- 23:46:00
--===============================================================================================================
--===============================================ПЕРЕМЕННЫЕ======================================================
g_graphIdent_Si_M05 = "Si_M05"
g_graphIdent_Si_M15 = "Si_M15"
g_graphIdent_Si_M30 = "Si_M30"
g_graphIdent_Si_M60 = "Si_M60"

g_graphIdent_SBRF_M05 = "SBRF_M05"
g_graphIdent_SBRF_M15 = "SBRF_M15"
g_graphIdent_SBRF_M30 = "SBRF_M30"
g_graphIdent_SBRF_M60 = "SBRF_M60"

g_graphIdent_GAZR_M05 = "GAZR_M05"
g_graphIdent_GAZR_M15 = "GAZR_M15"
g_graphIdent_GAZR_M30 = "GAZR_M30"
g_graphIdent_GAZR_M60 = "GAZR_M60"

g_graphIdent_BR_M05 = "BR_M05"
g_graphIdent_BR_M15 = "BR_M15"
g_graphIdent_BR_M30 = "BR_M30"
g_graphIdent_BR_M60 = "BR_M60"

g_ticker_Si   = "Si"
g_ticker_SBRF = "SBRF"
g_ticker_GAZR = "GAZR"
g_ticker_BR   = "BR"

g_timeframe_M05 = 5
g_timeframe_M15 = 15
g_timeframe_M30 = 30
g_timeframe_M60 = 60

g_scriptName = "CandleCollector"
g_numberCandlesLimit = 100 -- сколько свечек запрашиваем каждый раз
g_isRun     = true -- флаг работы скрипта
g_waitLimit = 3    -- пауза в секундах в алгоритме при наличии ошибок
g_wait      = g_waitLimit
g_message   = ""   -- сообщение от стратегии, которое будет выводиться в таблицу мониторинга
g_progress  = ""   -- строка для отображения прогресс-бара в таблице мониторинга
g_logFile   = getScriptPath().."\\Logs\\"..g_scriptName.."-"..StringCurrentDate()..".log" -- путь до файла с логами
--===============================================================================================================

-- главная функция скрипта, которая будет выполняться пока установлен флаг g_isRun
function main()   
	while g_isRun do
		Proccess()
		sleep(g_interval)
	end      	
end 

-- событие при запуске скрипта
function OnInit() 
	local logSource = g_scriptName..".OnInit()"
	g_isRun = true
	CreateTable() -- создаем таблицу мониторинга

	AddRow("Graph ID")
	AddRow("Ticker")
	AddRow("Timeframe")
	AddRow("Last candle")
	AddRow("Total candles")
	AddRow("Message")
	AddRow("Progress")
	AddRow("Modifided")
	AddRow("ServerTime")

	UpdateTableValue("Graph ID", string.format([[%s]], g_graphIdent_Si_M05))
	UpdateTableValue("Ticker", string.format([[%s]], g_ticker_Si))
	UpdateTableValue("Timeframe", string.format([[%s]], g_timeframe_M05))

	LogInfo({ 
		source = logSource, 
		text = g_messages["NNKA"], 
		guid = "dddaae3b-8f9d-4220-b7ec-82fe49809fa5" })
end 

-- событие при остановке скрипта
function OnStop() 
	local logSource = g_scriptName..".OnStop()"
	g_isRun = false
	DestroyTable(g_tableId)
	LogInfo({ 
		source = logSource, 
		text = g_messages["QZR4"], 
		guid = "3e40c6a8-eadd-4ac7-8f5a-e87feb4d666a" })	
end

-- алгоритм
function Proccess()	
	local startTime = os.clock()
	local logSource = g_scriptName..".Proccess()"
	
	-- вывод таблицы мониторинга заново, если закрылась
	if (IsWindowClosed(g_tableId)) then 
		CreateWindow(g_tableId) 
		InitTable()
	end	
			
	WaitingProcessErrors() -- пауза в работе и вывод сообщения об ошибке в таблицу мониторинга
	
	-- время торгов
	if (NoTradingTime() ~= 0) then 
		return 
	end 

	-- доступность БД
	if (DbNotAvailable() ~= 0) then 
		return 
	end 

	-- доступность сервера
	if (ServerNotAvailable() ~= 0) then 
		return 
	end 
	
	-- доступность свечек Si
	if (CandlesNotAvailable(g_graphIdent_Si_M05) ~= 0) then 
		return 
	end 
	
	local candles_Si_M05 = GetCandles(g_graphIdent_Si_M05)

	AddCandlesToDb(string.format([[candles_%s]], g_graphIdent_Si_M05), candles_Si_M05, g_timeframe_M05)

	local totalCandles_Si_M05 = TotalCandles(g_graphIdent_Si_M05)
	
	UpdateTableValue("Total candles", string.format([[%s]], totalCandles_Si_M05))

	UpdateTableValue("Last candle", string.format([[%s]], tostring(candles_Si_M05[#candles_Si_M05].close)))

	local modifidedDateTime = StringDateTimeFormat(os.date("*t", os.time()))
	UpdateTableValue("Modifided", modifidedDateTime)

	local endTime = os.clock()
    local totalTime = endTime - startTime
	g_messageText = tostring(MathRound(totalTime, 4)).." seconds..."
	UpdateTableValue("Message", g_messageText)
	ProgressToTable() -- рисуем палочку в прогресс-баре		
end