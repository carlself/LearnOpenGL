#version 330 core

in vec2 TexCoords;

out vec4 FragColor;

struct Material
{
    sampler2D diffuse_texture1;
};

uniform Material material;

void main()
{
    vec3 texColor = vec3(texture(material.diffuse_texture1, TexCoords));
    FragColor = vec4(texColor, 1.0);
}