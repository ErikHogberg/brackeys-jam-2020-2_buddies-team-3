function love.load() love.graphics.setColor(0, 0, 0) end

x, y, w, h = 10, 10, 10, 10

groundX, groundY, groundW, groundH = 0, 0, 100, 100
groundColor = {0.5, 0.1, 0}

viewAngle = 0;

function love.draw()
    local width = love.graphics.getWidth()
    local height = love.graphics.getHeight()

    love.graphics.print("This text is not black because of the line below", 100,
                        100)
    love.graphics.setColor(1, 0, 0)
    love.graphics.print("This text is red", 100, 200)

    love.graphics.print("WASD to move, E/Q to rotate", 100, 250)

    love.graphics.push()
    love.graphics.translate(width / 2, height / 2)
    love.graphics.rotate(viewAngle)
    love.graphics.translate(-width / 2, -height / 2)
    love.graphics.setColor(groundColor)

    local groundXResult = width / 2 - groundW / 2 + groundX
    local groundYResult = height / 2 - groundH / 2 + groundY
    love.graphics.rectangle("fill", groundXResult, groundYResult, groundW,
                            groundH)

    love.graphics.setColor(0, 1, 0)
    love.graphics.rectangle("fill", groundXResult + x, groundYResult + y, w, h)

    love.graphics.pop()

end


function love.focus(f) gameIsPaused = not f end

rotate = 0
dirX = 0
dirY = 0
speed = 100
function love.update(dt)
    if gameIsPaused then return end

    viewAngle = viewAngle + rotate * dt
    x = x + (dirX * math.cos(viewAngle) + dirY * math.sin(viewAngle)) * dt *
            speed
    y = y + (dirX * math.sin(-viewAngle) + dirY * math.cos(-viewAngle)) * dt *
            speed

    -- The rest of your love.update code goes here
end

function love.mousepressed(x, y, button, istouch)
    if button == 1 then
        --    imgx = x -- move image to where mouse clicked
        --    imgy = y
    end
end

function love.mousereleased(x, y, button, istouch)
    if button == 1 then
        fireSlingshot(x, y) -- this totally awesome custom function is defined elsewhere
    end
end

function love.keypressed(key)
    if key == 'w' then dirY = -1 end
    if key == 's' then dirY = 1 end
    if key == 'a' then dirX = -1 end
    if key == 'd' then dirX = 1 end
    if key == 'e' then rotate = 1 end
    if key == 'q' then rotate = -1 end
    if key == 'escape' then love.event.quit() end
end

function love.keyreleased(key)
    if key == 'w' or key == 's' then dirY = 0 end
    if key == 'a' or key == 'd' then dirX = 0 end
    if key == 'e' or key == 'q' then rotate = 0 end
end

function love.quit() print("Thanks for playing! Come back soon!") end

