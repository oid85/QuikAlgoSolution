--- описание:  преобразуем число в строку из двух символов
---            0 => 00
---            1 => 01
---            12 => 12
--- параметр:  num - число (тип параметра: number)
--- результат: строка из двух символов (тип результата: string)
function StringTwoDigit(num)
	local sign
	if (num < 0) then sign = "-" else sign = "" end
	num = math.abs(num)
	if (num == 0) then return sign.."00" end
	if (num <= 9) then return sign.."0"..tostring(num) end
	return sign..tostring(num)
end
	
--- описание:  символ с позицией i в строке s
---            если i > 0, то с начала строки
---            если i < 0, то с конца строки
--- параметр:  s - строка (тип параметра: string)
--- параметр:  i - позиция (тип параметра: number)
--- результат: символ (тип результата: string)
function SymbolByIndex(s, i)
	if (i == 0) then return nil	end
	if (math.abs(i) > string.len(s)) then return nil end
	return string.char(string.byte(s, i))
end

--- описание:  подстрока, находящаяся слева от подстроки middle
---            "qwe{}zxc" => "qwe" - слева от "{}"
--- параметр:  str - строка (тип параметра: string)
--- параметр:  middle - подстрока (тип параметра: string)
--- результат: подстрока (тип результата: string)
function SubstrLeft(str, middle)
	local index = string.find(str, middle)	
	if (index == nil) then return nil end	
	return string.sub(str, 0, index - 1)
end

--- описание:  подстрока, находящаяся справа от подстроки middle
---            "qwe{}zxc" => "zxc" - справа от "{}"
--- параметр:  str - строка (тип параметра: string)
--- параметр:  middle - подстрока (тип параметра: string)
--- результат: подстрока (тип результата: string)
function SubstrRight(str, middle)
	local index = string.find(str, middle)
	if (index == nil) then return nil end
	return string.sub(str, index + string.len(middle))
end

--- описание:  подстрока, заключенная между двумя подстроками
---            "qwe{asd}zxc" => "asd" - между подстроками "{" и "}"
--- параметр:  str - строка (тип параметра: string)
--- параметр:  b1 - подстрока (тип параметра: string)
--- параметр:  b2 - подстрока (тип параметра: string)
--- результат: подстрока (тип результата: string)
function SubstrBetween(str, b1, b2)
	local index = string.find(str, b1)	
	if (index == nil) then return nil end
	local res = string.sub(str, index + 1)
	
	index = string.find(res, b2)
	if (index == nil) then return nil end	
	return string.sub(res, 0, index - string.len(b2))
end

--- описание:  преобразует список элементов в строку с разделителями
--- параметр:  items - список элементов (тип параметра: table)
--- параметр:  separator - разделитель (тип параметра: string)
--- параметр:  brace - символ слева и справа (тип параметра: string)
--- результат: строка с разделителями (тип результата: string)
function StringJoin(items, separator, brace)
	local result = ""
	for i = 1, #items do		
		result = result..brace..items[i]..brace..separator
	end
	result = string.sub(result, 0, #result - 1)
	return result
end