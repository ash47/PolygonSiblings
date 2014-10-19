function init()

end

function updatePlayer(ply)
    local body = ply:getBody()
    body:ApplyImpulse(util:Vec2(0, 0.15), util:Vec2(0, 0))
end



