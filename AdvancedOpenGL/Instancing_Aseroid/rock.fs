#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

struct Material
{
    sampler2D texture_diffuse1;
};

uniform Material material;

void main()
{
    vec3 texColor = vec3(texture(material.texture_diffuse1, TexCoords));
    FragColor = vec4(texColor, 1.0);
}