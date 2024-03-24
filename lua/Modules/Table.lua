-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")

--- описание:  создание таблицы
--- параметр:  caption - заголовок таблицы (тип параметра: string)
--- результат: создание таблицы (тип результата: действие)
function CreateTable(caption)
	local logSource = "Table.CreateTable(caption)"
	g_tableId = AllocTable()												
	AddColumn (g_tableId, 1, "PARAMETERS", true, QTABLE_STRING_TYPE, 20)
	AddColumn (g_tableId, 2, "VALUES", true, QTABLE_STRING_TYPE, 55)
	CreateWindow(g_tableId)
	Clear(g_tableId)
	SetWindowPos(g_tableId, g_left, g_top, g_width, g_height) -- вывод таблицы (отступ слева, отступ сверху, ширина, высота)
	SetWindowCaption(g_tableId, tableName)                    -- текст в заголовке таблицы	
	LogTrace({ 
		source = logSource, 
		text = g_messages["YNNJ"], 
		guid = "b10d5e3f-d3ae-4e03-a698-90bab9e05a06" })	
end

--- описание:  удаление таблицы
--- параметр:  параметры отсутствуют
--- результат: удаление таблицы (тип результата: действие)
function DeleteTable()
	local logSource = "Table.DeleteTable()"
	DestroyTable(g_tableId)
	LogTrace({ 
		source = logSource, 
		text = g_messages["V8BW"], 
		guid = "846887e9-c416-4e8a-89c1-56152bd513ca" })	
end

--- описание:  добавление строки в конец таблицы
--- параметр:  description - описание строки (тип параметра: string)
--- результат: добавление строки в конец таблицы (тип результата: действие)
function AddRow(description)
	local logSource = "Table.AddRow(description)"
	InsertRow(g_tableId, -1)
	local rows, columns = GetTableSize(g_tableId)
	SetCell(g_tableId, rows, 1, description)
	LogTrace({ 
		source = logSource, 
		text = g_messages["2SOT"], 
		details = string.format([[ Details: Row name - (%s)]], tostring(description)), 
		guid = "cb47dabc-aeda-402b-9000-1da5b30d3bba" })	
end

--- описание:  обновить значение в таблице
--- параметр:  rowName - наименование строки (тип параметра: string)
--- параметр:  value - значение (тип параметра: string)
--- результат: обновление значения в таблице (тип результата: действие)
function UpdateTableValue(rowName, value)
	local logSource = "Table.UpdateTableValue(rowName, value)"
	
	local rows, columns = GetTableSize(g_tableId)
	
	for i = 1, rows do
		if (tostring(GetCell(g_tableId, i, 1).image) == rowName) then
			SetCell(g_tableId, i, 2, value)	
			
			-- true - включить трассировку значений
			-- false - выключить трассировку значений
			if (false) then
				LogDebug({ 
					source = logSource, 
					text = g_messages["D6K6"], 
					details = string.format([[ Details: Row name - (%s), Value - (%s)]], tostring(rowName), tostring(value)), 
					guid = "9d2f2332-2da1-4138-87ac-509e7f17ef1f" })
			end	

			return
		end
	end	
end

--- описание:  вывод индикатора работы алгоритма (прогресс-бар)
--- параметр:  параметры отсутствуют
--- результат: вывод индикатора работы алгоритма (прогресс-бар) в таблицу (тип результата: действие)
function ProgressToTable()
	if(string.len(g_progress) > 40) then
		g_progress = ""
	end	
	g_progress = g_progress.." | |"
	UpdateTableValue("Progress", g_progress)											
end