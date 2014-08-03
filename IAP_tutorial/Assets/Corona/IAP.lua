module(..., package.seeall);

-- this method searches for a newline symbol and cuts the prices strings to a table
local function cutString(text)
	local price = {} 
	local start = 1
	for i=1, 5 do
		if (string.find(text,"\n", start)) then
			local newLine = string.find(text,"\n",start) - 1 
			price[i] = string.sub(text,start,newLine)
			start = newLine + 2
		else
			price[i] = string.sub(text,start)
			break;
		end	
	end
	return price
end

-- I'm looking for the file that I created using C# method
function readPrices()
	local path = system.pathForFile( "Prices.txt", system.DocumentsDirectory);
	local file = io.open( path, "r" );
	local price
	if file then
	   local contents = file:read( "*a" );
	   io.close(file);	
	   price = cutString(contents)
	else
		-- if there's no file (i.e. testing on simulator) then I'm declaring temp values
		price = {"price1 lua", "price2 lua", "price3 lua"}
	end
	return price
end
