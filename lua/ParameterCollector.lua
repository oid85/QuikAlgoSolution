-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Check.lua")
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Math.lua")
dofile(getScriptPath().."\\Modules\\Money.lua")
dofile(getScriptPath().."\\Modules\\Security.lua")
dofile(getScriptPath().."\\Modules\\Table.lua")

--===============================================НАСТРОЙКИ=======================================================
-- расположение таблицы мониторинга
g_left   = 0   -- отступ слева
g_top    = 0   -- отступ сверху
g_width  = 400 -- ширина таблицы мониторинга
g_height = 200 -- высота таблицы мониторинга

g_interval = 3000 -- интервал опроса терминала в миллисекундах

-- начало и конец торговой сессии
g_startTime = 100100
g_endTime   = 234600
--===============================================================================================================
--===============================================ПЕРЕМЕННЫЕ======================================================
-- служебные переменные
g_account = "76200LG"

g_classCode = "SPBFUT"

g_securityCode_Si = "SiM0"

g_scriptName = "ParameterCollector"
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

	AddRow("FORTS Money")
	AddRow("SecurityCode")
	AddRow("Gaurant")
	AddRow("Message")
	AddRow("Progress")
	AddRow("Modifided")
	AddRow("ServerTime")
	
	UpdateTableValue("SecurityCode", string.format([[%s]], g_securityCode_Si))
	
	LogInfo({ 
		source = logSource, 
		text = g_messages["NNKA"], 
		guid = "e7e778fe-753e-4022-90a2-522ef2ff7600" })
end 

-- событие при остановке скрипта
function OnStop() 
	local logSource = g_scriptName..".OnStop()"
	g_isRun = false
	DestroyTable(g_tableId)
	LogInfo({ 
		source = logSource, 
		text = g_messages["QZR4"], 
		guid = "f9e72493-f7e5-49fb-893d-78fd67043061" })		
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
	
	if (NoTradingTime() ~= 0) then return end -- время торгов
	if (DbNotAvailable() ~= 0) then return end -- доступность БД
	if (ServerNotAvailable() ~= 0) then return end -- доступность сервера
	
	local money = GetMoneyForts(g_account)
	UpdateAccountInDb(g_account, money)
	AddMoneyFortsToDb(money)
	UpdateTableValue("FORTS Money", tostring(money))

	local gaurant1 = GetGuarant(g_classCode, g_securityCode_Si)

	UpdateGaurantInDb(g_classCode, g_securityCode_Si, gaurant1)	
	
	UpdateTableValue("Gaurant", string.format([[%s]], gaurant1))

	local modifidedDateTime = StringDateTimeFormat(os.date("*t", os.time()))
	UpdateTableValue("Modifided", modifidedDateTime)

	local endTime = os.clock()
    local totalTime = endTime - startTime
	g_messageText = tostring(MathRound(totalTime, 4)).." seconds..."
	UpdateTableValue("Message", g_messageText)
	ProgressToTable() -- рисуем палочку в прогресс-баре		
end