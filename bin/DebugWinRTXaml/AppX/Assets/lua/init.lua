function init()

end

local moveSpeed = 0.2
local jumpPower = 8

function updatePlayer(ply)
    local body = ply:getBody()
    local pos = body:GetPosition()
    local vel = body:GetLinearVelocity()

    if controls:leftDown() then
        body:ApplyImpulse(util:Vec2(-moveSpeed, 0), pos)
    end

    if controls:rightDown() then
        body:ApplyImpulse(util:Vec2(moveSpeed, 0), pos)
    end

    if controls:jumpDown() then
        if vel.Y < 0.1 and vel.Y > -0.1 then
            body:ApplyImpulse(util:Vec2(0, jumpPower), pos)
        end
    end
end



