-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Check.lua")
dofile(getScriptPath().."\\Modules\\Math.lua")

--===============================================НАСТРОЙКИ=======================================================
-- расположение таблицы мониторинга
g_left   = 0   -- отступ слева
g_top    = 400   -- отступ сверху
g_width  = 400 -- ширина таблицы мониторинга
g_height = 100 -- высота таблицы мониторинга

g_interval = 3000 -- интервал опроса терминала в миллисекундах

-- начало и конец торговой сессии
g_startTime = 100100
g_endTime   = 234600
--===============================================================================================================
--===============================================ПЕРЕМЕННЫЕ======================================================
-- служебные переменные
g_scriptName = "TradeCollector"
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

-- событие при появлении сделки в таблице сделок терминала Quik
function OnTrade(trade) 
	local startTime = os.clock()

	-- добавляем сделку в БД
	AddTradeToDb(trade) 

	local endTime = os.clock()
	local totalTime = endTime - startTime
	
	g_messageText = tostring(MathRound(totalTime, 4)).." seconds..."

	UpdateTableValue("Message", g_messageText)
end 

-- событие при запуске скрипта
function OnInit() 
	local logSource = g_scriptName..".OnInit()"
	g_isRun = true
	CreateTable() -- создаем таблицу мониторинга

	AddRow("Message")
	AddRow("Progress")
	
	LogInfo({ 
		source = logSource, 
		text = g_messages["NNKA"], 
		guid = "" })
end 

-- событие при остановке скрипта
function OnStop() 
	local logSource = g_scriptName..".OnStop()"
	g_isRun = false
	DestroyTable(g_tableId)
	LogInfo({ 
		source = logSource, 
		text = g_messages["QZR4"], 
		guid = "" })		
end

function Proccess() 
	local startTime = os.clock()	
	local endTime = os.clock()
    local totalTime = endTime - startTime
	g_messageText = tostring(MathRound(totalTime, 4)).." seconds..."
	UpdateTableValue("Message", g_messageText)
	ProgressToTable() -- рисуем палочку в прогресс-баре	  	
end 