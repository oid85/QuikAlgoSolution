-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Db.lua")
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")
dofile(getScriptPath().."\\Modules\\String.lua")
dofile(getScriptPath().."\\Modules\\Table.lua")
dofile(getScriptPath().."\\Modules\\Time.lua")

--- описание: сделка из БД в виде строки
--- параметр: candle - свеча (тип результата: table)
--- результат: свеча в виде строки (тип результата: string)
function TradeToString(trade)
	return string.format([[{%s %s %s %s %s %s %s}]], 
  StringDate(candle.datetime), StringTime(candle.datetime),
  tostring(candle.open), tostring(candle.close), tostring(candle.high), tostring(candle.low), tostring(candle.volume))
end

--- описание:  определение Id стратегии по комментарию к заявке или сделке
--- параметр:  comment - комментарий к заявке или сделке (тип параметра: string)
--- результат: Id стратегии (тип результата: number)
function GetStrategyIdFromComment(comment)
	local transId = tonumber(SubstrBetween(tostring(comment), "{", "}")) -- из комментария в сделке определяем ID транзакции номер сделки заключен между { и }
  
  if (transId == nil) then 
    return nil 
  end	
  
  return tonumber(math.floor(transId / 1000000.0))
end