#version 330 core 
out vec4 FragColor;

in vec3 ourColor;
in vec2 texCoord;

uniform vec3 objectColor;
uniform vec3 lightColor;


void main()
{
	FragColor = vec4(objectColor * lightColor, 1.0);
}