--- описание:  запись в файл с добавлением
--- параметр:  path - путь к файлу (тип параметра: string)
--- параметр:  line - записываемая строка (тип параметра: string)
--- результат: запись в файл с добавлением (тип результата: действие)
function FileAppendLine(path, line)
	local f = io.open(path, "a")
	
	if f == nil then 
		return nil
	end
	
	f: write(line.."\n")
	f: flush()
	f: close()
end

--- описание:  запись в файл без добавления
--- параметр:  path - путь к файлу (тип параметра: string)
--- параметр:  line - записываемая строка (тип параметра: string)
--- результат: запись в файл без добавления (тип результата: действие)
function FileWriteLine(path, line)
	local f = io.output(path)

	if f == nil then 
		return nil
	end

	f: write(line)
	f: flush()
	f: close()
end

--- описание:  построчное чтение из файла в таблицу
--- параметр:  path - путь к файлу (тип параметра: string)
--- результат: содержимое файла (тип результата: table)
function FileReadAllLines(path)
	local f = io.open(path, "r+")

	if f == nil then 
		return nil 
	end

	local res = {}
	local i = 0

	for line in f: lines() do
		res[i] = line
		i = i + 1
	end

	f: flush()
	f: close()

	return res
end

--- описание:  чтение из файла целиком
--- параметр:  path - путь к файлу (тип параметра: string)
--- результат: содержимое файла (тип результата: string)
function FileRead(path)
	local f = io.open(path, "r+")

	if f == nil then 
		return nil
	 end

	local res = ""
	local i = 0

	for line in f: lines() do
		res = res..line.."\n"
		i = i + 1
	end

	f: flush()
	f: close()
	
	return res
end