-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Db.lua")
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")

--- описание:  размер счета (фьючерсы)
--- параметр:  параметры отсутствуют
--- результат: значение размера счета (тип результата: number)
function GetMoneyForts(account)
	local logSource = "Money.GetMoneyForts()"
	
	if (account == nil) then
		LogError({ 
			source = logSource, 
			text = g_messages["3WFV"], 
			details = "account", 
			guid = "e6f54944-25ab-4412-a501-b3d3dc4dc9a5" })
		return nil
	end	

	local tableName = "FUTURES_CLIENT_LIMITS"

	local count = getNumberOf(tableName)	
	if (count == nil) then 
		LogError({ 
			source = logSource, 
			text = g_messages["PCC5"], 
			guid = "239728fb-810d-405d-9d44-ed35c6818e00" })
		return nil 
	end

	for i = 0, count - 1 do
		local limit = getItem(tableName, i)

		if (limit == nil) then 
			LogError({ 
				source = logSource, 
				text = g_messages["37WS"], 
				guid = "646aa26e-513e-4495-ae7f-eb6db32d8b7e" })
			return nil 
		end	

		if (limit.trdaccid == account and limit.limit_type == 0) then
			return tonumber(limit.cbplimit + limit.varmargin + limit.accruedint)
		end
	end
	
	return nil
end