-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Db.lua")
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")
dofile(getScriptPath().."\\Modules\\Order.lua")

--- описание:  округление до шага цены
--- параметр:  price - значение цены (тип параметра: number)
--- результат: значение цены, округленное до шага (тип результата: number)
function RoundToStep(price)
	local logSource = "Security.RoundToStep(price)"

	if (g_stepPrice == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["3XP2"], 
			details = "g_stepPrice", 
			guid = "a5e10a4e-2b2a-4924-817b-a2a0f4b9828b" })
		return nil
		end	
		
	if (g_stepPrice == 0) then
		LogError({ 
			source = logSource, 
			text = g_messages["1V21"], 
			details = "g_stepPrice", 
			guid = "956d129a-26cb-414e-8078-2b48e0f2fa86" })
		return nil
	end	    

	if (price == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["3WFV"], 
			details = "price", 
			guid = "c816553a-1958-4f04-9754-e917f399ae19" })
		return nil
	end			

	local ost = price % g_stepPrice -- остаток от деления	

	if (ost < g_stepPrice / 2) then
		return tonumber(math.floor(price / g_stepPrice) * g_stepPrice) -- округление вверх
	else 
		return tonumber(math.ceil(price / g_stepPrice) * g_stepPrice) -- округление вниз
	end
end

--- описание:  параметр инструмента
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- параметр:  parameter - код параметра (тип параметра: string)
--- результат: значение параметра инструмента (тип результата: number)
function GetSecurityParameter(classCode, securityCode, parameter)
	local logSource = "Security.GetSecurityParameter(classCode, securityCode, parameter)"
	
	if (classCode == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["3WFV"], 
			details = "classCode", 
			guid = "a6d672ec-c960-4f40-a874-2621cf3f8cf0" })
		return nil
	end	

	if (securityCode == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["3WFV"], 
			details = "securityCode", 
			guid = "0f5566cf-1843-4edd-93b7-d197634ce1cc" })
		return nil
		end	
		
	if (parameter == nil) then
		LogError({ 
			source = logSource, 
			test = g_messages["3WFV"], 
			details = "parameter", 
			guid = "957780d6-d73d-406b-9f9d-ffb27d1e1f46" })
		return nil
	end	   

	return tonumber(getParamEx(classCode, securityCode, parameter).param_value)
end

--- описание:  последняя цена инструмента
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: значение последней цены инструмента (тип результата: number)
function GetLastPrice(classCode, securityCode)  
	return GetSecurityParameter(classCode, securityCode, "LAST")
end

--- описание:  размер ГО фьючерса
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: значение размера ГО фьючерса (тип результата: number)
function GetGuarant(classCode, securityCode)
	return GetSecurityParameter(classCode, securityCode, "BUYDEPO")
end

--- описание:  корректировка позиции (позиция считывается из Quik)
--- параметр:  targetPosition - расчетная позиция (тип параметра: number)
--- параметр:  currentPosition - текущая позиция (тип параметра: number)
--- параметр:  price - цена заявки (тип параметра: number)
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: результат корректировки позиции (тип результата: number)
---            0 - корректировка позиции не выполнялась
---            1 - выполнена докупка
---           -1 - выполнена допродажа
---           nil - корректировка позиции не может быть выполнена
function CorrectPositionQuik(targetPosition, currentPosition, price, classCode, securityCode)
	local logSource = "Security.CorrectPositionQuik(targetPosition, currentPosition, price, classCode, securityCode)"
	
	local quantity = targetPosition - currentPosition	
	
	-- корректировать позицию не нужно
	if (quantity == 0) then 
		return 0 
	end 
	
	local orderParameters = 
	{
		account = g_account,
		traderId = g_traderId,
		classCode = classCode,
		securityCode = securityCode,
		price = price,
		quantity = quantity
	}
	
	if (quantity > 0) then -- нужно докупить	
		LogInfo({ 
			source = logSource, 
			test = g_messages["D0XP"], 
			details = string.format([[%s, quantity %s, price %s]], tostring(securityCode), tostring(quantity), tostring(price)), 
			guid = "d08581b1-cd9f-408c-9550-0e8ffea55d53" })	
		BuyAtLimit(orderParameters)
		return 1
	end
	if (quantity < 0) then -- нужно допродать
		LogInfo({ 
			source = logSource, 
			test = g_messages["AIPM"], 
			details = string.format([[%s, quantity %s, price %s]], tostring(securityCode), tostring(quantity), tostring(price)), 
			guid = "15e20851-4482-424a-b0ae-76bf8b2229c9" })		
		SellAtLimit(orderParameters)
		return -1
	end
end

--- описание:  размер текущей позиции из Quik
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: результат размера текущей позиции из Quik (тип результата: number)
function CurrentPositionQuik(classCode, securityCode)
	local isFutures = IsFutures(classCode)	
	if (isFutures == 1) then
		return CurrentPositionFutures(classCode, securityCode)
	end
	local isStock = IsStock(classCode)
	if (isStock == 1) then
		return CurrentPositionStock(classCode, securityCode)
	end
	return 0 
end

--- описание:  размер текущей позиции по фьючерсам
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: результат размера текущей позиции по фьючерсам (тип результата: number)
function CurrentPositionFutures(classCode, securityCode)
	local tableName = "FUTURES_CLIENT_HOLDING"
	local count = getNumberOf(tableName)
	
	if (count == nil) then 
		return 0 
	end
	
	if (count ~= nil)then	
		for i = 0, count - 1 do
			local row = getItem(tableName, i)
			if(row ~= nil and row.sec_code == securityCode and row.trdaccid == g_account) then
				return tonumber(row.totalnet)
			end
		end
	end	
	
	return 0
end

--- описание:  размер текущей позиции по акциям
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- параметр:  securityCode - код инструмента (тип параметра: string)
--- результат: результат размера текущей позиции по акциям (тип результата: number)
function CurrentPositionStock(classCode, securityCode)
	local limitKind = 2
	local balanceType = 1
	
	local isStock = IsStock(classCode)
	
	local tableName = "DEPO_LIMITS"
	local count = getNumberOf(tableName)
	
	if (count == nil) then 
		return 0 
	end	
	
	if (count == 0) then 
		return 0 			
	end
	
	if (count == 1) then 
		local lot = tonumber(getParamEx(classCode, securityCode, "LOTSIZE").param_value)
		local limit = getItem(tableName, 0)
		if limit.sec_code == securityCode and limit.trdaccid == g_account and limit.limit_kind == limitKind then 
		   if balanceType == 1 then
			  return limit.currentbal
		   else
			  return limit.currentbal / lot
		   end
		end			
	end
	if (count > 1) then 
		local lot = tonumber(getParamEx(classCode, securityCode, "LOTSIZE").param_value)
		for i = 0, count - 1 do
		   local limit = getItem(tableName, i)
		   if limit.sec_code == securityCode and limit.trdaccid == g_account and limit.limit_kind == limitKind then 
			  if balanceType == 1 then
				 return limit.currentbal
			  else
				 return limit.currentbal / lot
			  end
		   end
		end			
	end	
	
	return 0
end

--- описание: является ли инструмент фьючерсом
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- результат: 1 - является, 0 - не является (тип результата: number)
function IsFutures(classCode)
	if (classCode == "SPBFUT") then
		return 1
	end
	return 0
end

--- описание: является ли инструмент акцией
--- параметр:  classCode - класс инструмента (тип параметра: string)
--- результат: 1 - является, 0 - не является (тип результата: number)
function IsStock(classCode)
	if (classCode == "TQBR") then
		return 1
	end
	return 0
end
