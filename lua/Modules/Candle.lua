-- подключаем нужные файлы с функциями
dofile(getScriptPath().."\\Modules\\Db.lua")
dofile(getScriptPath().."\\Modules\\Log.lua")
dofile(getScriptPath().."\\Modules\\Message.lua")
dofile(getScriptPath().."\\Modules\\Table.lua")
dofile(getScriptPath().."\\Modules\\Time.lua")

--- описание:  считать свечи из Quik
--- параметр:  identPrice - идентификатор графика цены (тип параметра: string)
--- результат: свечи (тип результата: table)
function GetCandles(identPrice)
    local logSource = "Candles.GetCandles(identPrice)"
    
    if (identPrice == nil) then
      LogError({ 
        source = logSource, 
        text = g_messages["3WFV"], 
        details = "identPrice", 
        guid = "eb40fa88-0055-473d-9687-24d02d32d5ab" })
      return nil
    end    
    
    local count = getNumCandles(identPrice)	
	  if (count == nil) then 
      LogError({ 
        source = logSource, 
        text = g_messages["59X5"], 
        details = string.format([[identPrice = %s]], tostring(identPrice)), 
        guid = "86c77e09-f044-434f-a02d-acc6a3718de7" })
      return nil 
    end    
    
    local n = g_numberCandlesLimit
    if (n > count) then 
          n = count
          LogInfo({ 
            source = logSource, 
            text = g_messages["6LF3"], 
            details = string.format([[n = %s, count = %s]], tostring(n), tostring(count)), 
            guid = "00ce7eee-0464-4045-9111-f62731ba6513" })
    end	
    
    local candles, k, _ = getCandlesByIndex(identPrice, 0, count - n, n)
    if (k ~= n) then
      LogError({ 
        source = logSource, 
        text = g_messages["7EVT"], 
        guid = "4072d93f-8479-48ef-b7e8-a4b75789dfb2" })
      return nil 
    end
    
    if (k == 0) then
      LogError({ 
        source = logSource, 
        text = g_messages["JC3M"], 
        guid = "74ba6871-11f2-47a9-a8a3-58ea44ba1fb2" })
      return nil 
    end
    
    if (candles == nil) then
      LogError({ 
        source = logSource, 
        text = g_messages["OJJ2"], 
        guid = "1ac3d4d3-754c-41ac-b41f-f276d3f4c83d" })
      return nil 
    end	

    return candles
end

--- описание: свеча в виде строки
--- параметр: candle - свеча (тип результата: table)
--- результат: свеча в виде строки (тип результата: string)
function CandleToString(candle)
	return string.format([[{%s, %s, %s, %s, %s, %s}]], 
  StringDateTimeFormat(candle.datetime),
  tostring(candle.open), tostring(candle.close), tostring(candle.high), tostring(candle.low), tostring(candle.volume))
end

--- описание: количество свечей в БД
--- параметр: identPrice - идентификатор графика цены (тип параметра: string)
--- результат: количество свечей в БД (тип результата: number)
function TotalCandles(identPrice)
  return CountRowsOfDbTable(string.format([[candles_%s]], identPrice))
end