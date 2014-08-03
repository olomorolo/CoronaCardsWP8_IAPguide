module(..., package.seeall);

-- this method cuts searches for a newline symbol and cuts the string to a table
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
		price = {"price1", "price2", "price3"}
	end
	return price
end