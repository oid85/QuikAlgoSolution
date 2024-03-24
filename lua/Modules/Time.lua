-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\String.lua")

--- описание:  дата в формате yyyy-MM-dd HH:mm:ss (строкой)
--- параметр:  datetime - дата (тип параметра: table)
--- результат: дата в формате yyyyMMdd (строкой) (тип результата: string)
function StringDateTimeFormat(datetime)
	return string.format([[%s-%s-%s %s:%s:%s]], 
						StringTwoDigit(datetime.year), StringTwoDigit(datetime.month), StringTwoDigit(datetime.day), 
						StringTwoDigit(datetime.hour), StringTwoDigit(datetime.min), StringTwoDigit(datetime.sec))
end

--- описание:  дата в формате yyyyMMdd (строкой)
--- параметр:  datetime - дата (тип параметра: table)
--- результат: дата в формате yyyyMMdd (строкой) (тип результата: string)
function StringDate(datetime)
	return tostring(datetime.year)..StringTwoDigit(datetime.month)..StringTwoDigit(datetime.day)
end

--- описание:  дата в формате yyyyMMdd (числом)
--- параметр:  datetime - дата (тип параметра: table)
--- результат: дата в формате yyyyMMdd (числом) (тип результата: number)
function NumberDate(datetime)	
	return tonumber(StringDate(datetime))
end

--- описание:  время в формате HHmmss (строкой)
--- параметр:  datetime - время (тип параметра: table)
--- результат: время в формате HHmmss (строкой) (тип результата: string)
function StringTime(datetime)
	return StringTwoDigit(datetime.hour)..StringTwoDigit(datetime.min)..StringTwoDigit(datetime.sec)
end

--- описание:  время в формате HHmmss (числом)
--- параметр:  datetime - время (тип параметра: table)
--- результат: время в формате HHmmss (числом) (тип результата: number)
function NumberTime(datetime)
	return tonumber(StringTime(datetime))
end

--- описание:  текущая дата в формате yyyyMMdd (строкой)
--- параметр:  параметры отсутствуют
--- результат: текущая дата в формате yyyyMMdd (строкой) (тип результата: string)
function StringCurrentDate()
	local datetime = os.date("*t", os.time())
	return tostring(datetime.year)..StringTwoDigit(datetime.month)..StringTwoDigit(datetime.day)
end

--- описание:  текущая дата в формате yyyyMMdd (числом)
--- параметр:  datetime - текущая дата (тип параметра: table)
--- результат: текущая дата в формате yyyyMMdd (числом) (тип результата: number)
function NumberCurrentDate()
	return tonumber(StringCurrentDate())
end

--- описание:  текущее время в формате HHmmss (строкой)
--- параметр:  параметры отсутствуют
--- результат: текущее время в формате HHmmss (строкой) (тип результата: string)
function StringCurrentTime()
	local datetime = os.date("*t", os.time())
	return StringTwoDigit(datetime.hour)..StringTwoDigit(datetime.min)..StringTwoDigit(datetime.sec)
end

--- описание:  текущее время в формате HHmmss (числом)
--- параметр:  параметры отсутствуют
--- результат: текущее время в формате HHmmss (числом) (тип результата: number)
function NumberCurrentTime()
	return tonumber(StringCurrentTime())
end

--- описание:  дата сервера в формате yyyyMMdd (строкой)
--- параметр:  параметры отсутствуют
--- результат: дата сервера в формате yyyyMMdd (строкой) (тип результата: string)
function StringServerDate()
	local serverTime = getInfoParam("SERVERTIME")
	return StringDate(serverTime)
end

--- описание:  дата сервера в формате yyyyMMdd (числом)
--- параметр:  параметры отсутствуют
--- результат: дата сервера в формате yyyyMMdd (числом) (тип результата: number)
function NumberServerDate()
	return tonumber(StringServerDate())
end

--- описание:  время сервера в формате HHmmss (строкой)
--- параметр:  параметры отсутствуют
--- результат: время сервера в формате HHmmss (строкой) (тип результата: string)
function StringServerTime()
	local serverTime = getInfoParam("SERVERTIME")
	return StringTime(serverTime)
end

--- описание:  время сервера в формате HHmmss (числом)
--- параметр:  параметры отсутствуют
--- результат: время сервера в формате HHmmss (числом) (тип результата: number)
function NumberServerTime()
	return tonumber(StringServerTime())
end