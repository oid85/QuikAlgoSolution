--- описание:  запуск внешней программы
--- параметр:  programm - путь (или имя) программы (тип параметра: string)
--- параметр:  parameter - параметр запуска программы (тип параметра: string)
--- результат: запуск внешней программы (тип результата: действие)
function RunProgram(program, parameter)
	local cmd = string.format([[start %s]], program)
	if (parameter ~= nil) then
		cmd = cmd.." "..parameter
	end
	os.execute(cmd)
end

--- описание:  пауза
--- параметр:  milliseconds - количество миллисекунд (тип параметра: number)
--- результат: пауза (тип результата: действие)
function Sleep(milliseconds)
    local timer = os.clock() * 1000
    while os.clock() * 1000 - timer <= milliseconds do 
		-- ничего не делаем
	end
end