#version 330 core

out vec4 FragColor;

in vec3 Position;
in vec3 Normal;

uniform vec3 viewPos;
uniform samplerCube skybox;

void main()
{
    vec3 I = normalize(Position - viewPos);
    vec3 R = reflect(I, normalize(Normal));
    FragColor = vec4(texture(skybox, R).rgb, 1.0);
}