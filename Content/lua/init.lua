local moveSpeed = 15
local jumpPower = 8
local attackCooldown = 1
local attackDuration = 0.1
local attackDamage = 80
local attackPower = 0.5

-- Create blocky
game:registerCharacter('blocky', function(ply) -- Init
    -- Create the data field
    ply.data = {
        canJump = true,
        attackCooldown = 0
    }
    end, function(gameTime, ply) -- Update
    -- Grab useful vars
    local playerID = ply:getPlayerID()
    local data = ply.data

    -- Workout the change since the last call
    local dt = gameTime.ElapsedGameTime.Milliseconds/1000

    -- AI Time
    if game:isAI(playerID) then
        -- Target selection
        if not data.target or data.newTargetTime <= 0 then
            data.target = game:findClosePlayer(ply)
            data.newTargetTime = 2
        end

        -- Lower the timer for the next target
        data.newTargetTime = data.newTargetTime - dt

        -- Find the closest player
        local otherPly = data.target

        -- Ensure we got a player
        if otherPly then
            -- Can we attack?
            if data.attackCooldown <= 0 then
                -- Check if we should attack
                local dist = game:getDistance(ply, otherPly)
                if dist < 1 then
                    data.attackCooldown = attackCooldown

                    -- Do the attack
                    ply:createAttack(1, 1, util:Vec2(0, 0), 0, attackDamage, attackPower)
                end
            end

            -- Move towards the player
            local myPos = ply:getPosition()

            -- Stay near the center
            if myPos.X < -10 then
                -- Move right
                ply:ApplyForce(util:Vec2(moveSpeed*dt, 0))
            elseif myPos.X > 10 then
                -- Move left
                ply:ApplyForce(util:Vec2(-moveSpeed*dt, 0))
            else
                local theirPos = otherPly:getPosition()

                if myPos.X < theirPos.X then
                    -- Move right
                    ply:ApplyForce(util:Vec2(moveSpeed*dt, 0))
                else
                    -- Move left
                    ply:ApplyForce(util:Vec2(-moveSpeed*dt, 0))
                end
            end

            -- Randomly jump to make it fun to watch
            if game:getRandom() < 0.1 then
                if ply.touchingGround > 0 then
                    -- Do the jump
                    ply:ApplyForce(util:Vec2(0, jumpPower))
                end
            end
        end
    else
        -- Left Movement
        if controls:leftDown(playerID) then
            ply:ApplyForce(util:Vec2(-moveSpeed*dt, 0))
        end

        -- Right movement
        if controls:rightDown(playerID) then
            ply:ApplyForce(util:Vec2(moveSpeed*dt, 0))
        end

        -- Jumping
        if controls:jumpDown(playerID) then
            if data.canJump and ply.touchingGround > 0 then
                -- Disable jumping
                data.canJump = false

                -- Do the jump
                ply:ApplyForce(util:Vec2(0, jumpPower))
            end
        else
            -- Enable jumping again
            data.canJump = true
        end

        -- Attacking
        if controls:attackDown(playerID) then
            if data.attackCooldown <= 0 then
                data.attackCooldown = attackCooldown

                -- Do the attack
                ply:createAttack(1, 1, util:Vec2(0, 0), 0, attackDamage, attackPower)
            end
        end
    end

    -- Cooldown attack
    if data.attackCooldown > 0 then
        data.attackCooldown = data.attackCooldown - dt

        ply:setScale(1 + data.attackCooldown/attackCooldown)

        if data.attackCooldown < attackCooldown - attackDuration then
            -- Remove the attack
            ply:removeAttack()
        end
    else
        ply:setScale(1)
    end
end)
