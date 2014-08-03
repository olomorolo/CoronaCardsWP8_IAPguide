local composer = require("composer");
local scene = composer.newScene()

local _W = display.viewableContentWidth
local _H = display.viewableContentHeight

function scene:create( event )

	local sceneGroup = self.view

	-- these are button groups including button rectangles and button labels
	local buyBtnGroup = {}
	local buyBtn = {}

	local function buyProductOne(event)
		print("attempting to buy Product One")
  	 	Runtime:dispatchEvent( {name = "buyProductOne"} ) 
	end
	
	local function buyProductTwo(event)
		print("attempting to buy Product Two")
	  	Runtime:dispatchEvent( {name = "buyProductTwo"} ) 
	end
	
	local function buyProductThree(event)
		print("attempting to buy Product Three")
  	 	Runtime:dispatchEvent( {name = "buyProductThree"} ) 
	end

	-- I'm making IAP buttons
	for i = 1, 3 do

		buyBtnGroup[i] = display.newGroup( )
		buyBtnGroup[i].anchorChildren = true
		buyBtnGroup[i].x, buyBtnGroup[i].y = _W *0.5, i*140

		buyBtn[i] = display.newRect( 0, 0, 200, 100 )


		function buyBtn:touch(e)
			if e.phase == "ended" then				
				local name1 = tostring(e.target)
				for j=1, #buyBtn do
					local name2 = tostring(buyBtn[j])
					if name1 == name2 then
						if j == 1 then
							buyProductOne(e)
						elseif j == 2 then
							buyProductTwo(e)
						elseif j == 3 then
							buyProductThree(e)
						end
						break
					end
				end
			end
			return true			
		end
		
		buyBtn[i]:addEventListener( "touch", buyBtn )

		local btnLabel = display.newText( "Product No ".. i, 0, -10, system.nativeFont, 12 )	
		btnLabel:setFillColor(0,0,0)

		local btnPrice = display.newText( composer.state.price[i], 0, 10, system.nativeFont, 17 )	
		btnPrice:setFillColor(0,0,0)


		buyBtnGroup[i]:insert( buyBtn[i] )
		buyBtnGroup[i]:insert(btnLabel)
		buyBtnGroup[i]:insert(btnPrice)
		
		sceneGroup:insert(buyBtnGroup[i])	
	end	

end

---------------------------------------------------------------------------------

scene:addEventListener( "create", scene )

---------------------------------------------------------------------------------

return scene