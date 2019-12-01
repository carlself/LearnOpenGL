#version 330 core

out vec4 FragColor;

uniform sampler2D screenTexture;

in vec2 TexCoords;

void main()
{
    FragColor = texture(screenTexture, TexCoords);
    float average = 0.2125 * FragColor.r + 0.7152 * FragColor.g + 0.0722 * FragColor.b;
    FragColor = vec4(average, average, average, 1.0);
}