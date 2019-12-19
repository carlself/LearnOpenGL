#version 330 core

out vec4 FragColor;

in vec3 Position;
in vec3 Normal;

uniform vec3 viewPos;
uniform samplerCube skybox;

void main()
{
    vec3 I = normalize(Position - viewPos);
    vec3 R = refract(I, normalize(Normal), 1.0/1.52);
    FragColor = vec4(texture(skybox, R).rgb, 1.0);
}