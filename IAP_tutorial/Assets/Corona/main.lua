local composer = require("composer");
composer.state = {}
-- requiring a lua IAP class to read prices from file
local IAP = require("IAP");

display.setStatusBar( display.HiddenStatusBar )

system.setTapDelay(0.05)

local function onSystem(e)
	if e.type == "applicationStart" then
		--fetching the prices
		composer.state.price = IAP.readPrices() 
	elseif(e.type == "applicationSuspend") then

	elseif(e.type == "applicationResume") then

	elseif(e.type == "applicationExit") then
	end
end

Runtime:addEventListener("system",onSystem);

--- declare C# event listeners:

local function onPurchaseError(event)
	print("an error ocurred")
end
Runtime:addEventListener("purchaseError", onPurchaseError)

local function onPurchase(event)
	-- I found that sometimes I didn't get the event property instantly after receiving an event so I make a timer to check if I can fetch the property:
	local eventPropertyTimer
	eventPropertyTimer = timer.performWithDelay( 100, function(e)
		-- fetching the property 'coins'
		if event.coins then
			local coinsPurchased = event.coins
			print("you've purchased: ".. coinsPurchased .. " coins!")
			-- cancel the timer:
			timer.cancel( eventPropertyTimer )
			eventPropertyTimer = nil
		end
		end,-1)
end
Runtime:addEventListener("purchase", onPurchase)

--- C# END

-- I'm anticipating the scene load for 3s to make sure that the prices are already fetched from Windows Phone Store.
-- I'm doing this only because I'm displaying the prices instantly after application load. You won't need to do this
-- if you don't display the prices on screen in the first visible scene.
local gotoSceneTimer
gotoSceneTimer = timer.performWithDelay( 3000, function(e) 
	composer.gotoScene("menu");
	timer.cancel( gotoSceneTimer )
	gotoSceneTimer = nil
end,1)
