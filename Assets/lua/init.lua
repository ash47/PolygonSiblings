local moveSpeed = 15
local jumpPower = 8

-- Create blocky
game:registerCharacter('blocky', function(ply) -- Init
        -- Create the data field
        ply.data = {
            canJump = true
        }
    end, function(gameTime, ply) -- Update
    -- Grab useful vars
    local body = ply:getBody()
    local pos = body:GetPosition()
    local vel = body:GetLinearVelocity()
    local playerID = ply:getPlayerID()
    local data = ply.data

    -- Workout the change since the last call
    local dt = gameTime.ElapsedGameTime.Milliseconds/1000

    -- Left Movement
    if controls:leftDown(playerID) then
        body:ApplyImpulse(util:Vec2(-moveSpeed*dt, 0), pos)
    end

    -- Right movement
    if controls:rightDown(playerID) then
        body:ApplyImpulse(util:Vec2(moveSpeed*dt, 0), pos)
    end

    -- Jumping
    if controls:jumpDown(playerID) then
        if data.canJump and ply.touchingGround > 0 then
            -- Disable jumping
            data.canJump = false

            -- Do the jump
            body:ApplyImpulse(util:Vec2(0, jumpPower), pos)
        end
    else
        -- Enable jumping again
        data.canJump = true
    end
end)
