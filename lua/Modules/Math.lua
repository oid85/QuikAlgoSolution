--- описание:  округлить число
--- параметр:  number - число (тип параметра: number)
--- параметр:  digits - количество знаков после запятой (тип параметра: number)
--- результат: округленное число (тип результата: number)
function MathRound(number, digits)
    digits = math.pow(10, digits or 0)
    number = number * digits
    
	if (number >= 0) then
		number = math.floor(number + 0.5) 
	else 
		number = math.ceil(number - 0.5) 
	end
    
	return number / digits
end

--- описание:  проверка установки бита по номеру (нумерация с 0)
--- параметр:  flags - слово (тип параметра: number)
--- параметр:  bitNum - номер бита в слове (тип параметра: number)
--- результат: установлен ли бит в позиции (тип результата: bool)
function BitIsTrue(flags, bitNum)
	return flags % (2 ^ (bitNum + 1)) >= 2 ^ bitNum
end

--- описание:  перевод числа в двоичную систему
--- параметр:  num - число в десятичной системе (тип параметра: number)
--- результат: число в двоичной системе (тип результата: string)
function ToBin(num)
    local tmp = {}
    repeat
        tmp[#tmp + 1] = num % 2
        num = math.floor(num / 2)
    until num == 0
    return table.concat(tmp): reverse()
end