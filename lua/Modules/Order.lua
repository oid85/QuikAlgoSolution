-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")

--- описание:  отправка лимитной заявки на покупку
--- параметр:  orderParameters - параметры заявки (тип параметра: table)
--- результат: результат отправки лимитной заявки на покупку (тип результата: number)
---            0   - заявка исполнилась
---            1   - заявка исполнилась
---            nil - заявка не может исполниться
function BuyAtLimit(orderParameters)
	orderParameters.operation = "B"
	return LimitOrder(orderParameters)
end

--- описание:  отправка лимитной заявки на продажу
--- параметр:  orderParameters - параметры заявки (тип параметра: table)
--- результат: результат отправки лимитной заявки на продажу (тип результата: number)
---            0   - заявка исполнилась
---            1   - заявка исполнилась
---            nil - заявка не может исполниться
function SellAtLimit(orderParameters)
	orderParameters.operation = "S"
	return LimitOrder(orderParameters)
end

--- описание:  отправка лимитной заявки
--- параметр:  orderParameters - параметры заявки (тип параметра: table)
--- результат: результат отправки лимитной заявки (тип результата: number)
---            0   - заявка исполнилась
---            1   - заявка исполнилась
---            nil - заявка не может исполниться
function LimitOrder(orderParameters)
	local logSource = "Order.LimitOrder(orderParameters)"

	if (orderParameters.account == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["RWEB"], 
			guid = "e6e279a0-e7f1-439c-918d-b3a530253e42" })
		return nil
	end		

	if (orderParameters.traderId == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["OA7E"], 
			guid = "bf1365f9-4f0a-4e82-bfb0-81e357ae4fd7" })
		return nil
	end		

	if (orderParameters.classCode == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["FQCD"], 
			guid = "c211af76-00f0-4fb4-bed4-484f0f4f2d17" })
		return nil
	end	

	if (orderParameters.securityCode == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["ZJON"], 
			guid = "f22c62e2-8241-4e87-a5c8-1b767858ebf3" })
		return nil
	end

	if (orderParameters.price == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["MN58"], 
			guid = "1b2fa2dc-0080-40a9-ad57-ca77dcd09f17" })
		return nil
	end

	if (orderParameters.quantity == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["S3MV"], 
			guid = "3c261222-055d-4107-b80f-f500e70f0b8e" })
		return nil
	end

	if (orderParameters.operation == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["GBC4"], 
			guid = "e2a46e0b-8c5e-48c9-89a4-89c78d477169" })
		return nil
	end

	local transId = GetTransId(orderParameters.traderId)

	local transaction = 
	{
		["ACTION"]      = "NEW_ORDER",
		["TRANS_ID"]    = tostring(transId),
		["ACCOUNT"]     = orderParameters.account,
		["SECCODE"]     = orderParameters.securityCode,		
		["CLASSCODE"]   = orderParameters.classCode,
		["CLIENT_CODE"] = "{"..tostring(transId).."}",		
		["PRICE"]       = tostring(orderParameters.price),
		["QUANTITY"]    = tostring(math.abs(orderParameters.quantity)),
		["OPERATION"]   = orderParameters.operation,
		["TYPE"]        = "L"
	}

	local res = sendTransaction(transaction)

	local pause = 500 -- величина паузы в мс
	sleep(pause) -- делаем паузу, чтобы транзакция "улетела" в терминал	
	
	local timeout = 10000 -- величина тайм-аута в мс
	sleep(pause) -- выжидаем таймаут, чтобы заявка исполнилась
	
	-- снимаем остаток, который неисполнился
	local countOrders = getNumberOf("ORDERS") -- общее количество заявок
	for i = 0, countOrders - 1 do
		local order = getItem("ORDERS", i)		
		if (BitIsTrue(order.flags, 0) and order.balance > 0) then -- если заявка активна		
			transId = GetTransId(orderParameters.traderId)
			transaction = 
			{
				["TRANS_ID"]  = tostring(transId),
				["ACTION"]    = "KILL_ORDER",
				["SECCODE"]   = orderParameters.securityCode,
				["CLASSCODE"] = orderParameters.classCode,
				["ORDER_KEY"] = tostring(order.order_num)
			}
			res = sendTransaction(transaction)					
		end		
	end
	return 1
end

--- описание:  сгенерировать ID транзакции
--- параметр:  strategyId - ID стратегии (тип параметра: number)
--- результат: ID транзакции (тип результата: number)
function GetTransId(strategyId)
	local logSource = "Order.GetTransId()"

	if (strategyId == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["OA7E"], 
			guid = "088e98c1-34ed-43ae-ac5c-4bae4e9da3ce" })
		return nil
	end

	return strategyId * 1000000 + NumberCurrentTime() -- к значению Id стратегии добавим текущее время формата HHmmss
end