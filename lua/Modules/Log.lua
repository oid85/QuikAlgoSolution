-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\File.lua")
dofile(getScriptPath().."\\Modules\\Db.lua")

g_logInfo  = true -- писать информационные сообщения
g_logDebug = false -- писать отладочные сообщения
g_logError = true -- писать сообщения об ошибках
g_logTrace = false -- писать сообщения трассировки

g_logToFile = false  -- писать логи в файл
g_logToDb   = true  -- писать логи в БД

--- описание:  запись лог сообщения
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись сообщения(тип результата: действие)
function Log(message)
	message.datetime = StringDateTimeFormat(os.date("*t", os.time()))
	message.source = g_scriptName.."."..tostring(message.source)
	LogToFile(message)
	LogToDb(message)
end

--- описание:  запись сообщения в лог-файл
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись сообщения в лог-файл (тип результата: действие)
function LogToFile(message)
	if (g_logToFile) then
		FileAppendLine(g_logFile, string.format([[%s | %s | %s | %s | %s | %s]],
		tostring(message.datetime),
		tostring(message.type),
		tostring(message.source),
		tostring(message.text),
		tostring(message.details),
		tostring(message.guid)))
	end
end

--- описание:  запись сообщения в БД
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись сообщения в лог-файл (тип результата: действие)
function LogToDb(message)
	if (g_logToDb) then
		AddLogMessageToDb(message)
	end	
end

--- описание:  запись информационного сообщения в лог
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись информационного сообщения в лог-файл (тип результата: действие)
function LogInfo(message)
	if (g_logInfo) then
		message.type = "INFO"
		Log(message) 
	end	
end

--- описание:  запись отладочного сообщения в лог
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись отладочного сообщения в лог-файл (тип результата: действие)
function LogDebug(message)
	if (g_logDebug) then 
		message.type = "DEBUG"
		Log(message) 
	end
end

--- описание:  запись сообщения об ошибке в лог
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись сообщения об ошибке в лог-файл (тип результата: действие)
function LogError(message)
	if (g_logError) then 
		message.type = "ERROR"
		Log(message) 
	end
end

--- описание:  запись сообщения о работе алгоритма в лог
--- параметр:  message - сообщениe (тип параметра: table)
--- результат: запись сообщения о работе алгоритма в лог (тип результата: действие)
function LogTrace(message)
	if (g_logTrace) then 
		message.type = "TRACE"
		Log(message) 
	end
end