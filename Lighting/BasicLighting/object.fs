#version 330 core 

out vec4 FragColor;

uniform vec3 objectColor;
uniform vec3 lightColor;

uniform vec3 lightPos;
uniform vec3 viewPos;

in vec3 Normal;
in vec3 FragPos;


void main()
{
	float ambientStrength = 0.1;
	vec3 ambient = ambientStrength * lightColor;
	
	vec3 lightDir = normalize(lightPos - FragPos);

	vec3 diffuse = max(dot(lightDir, Normal), 0.0) * lightColor;

    float specularStrength = 0.5;

	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, Normal);
	vec3 specular = pow(max(dot(reflectDir, viewDir), 0), 128) * specularStrength * lightColor;

	

	vec3 result = (ambient  + diffuse + specular) * objectColor;
	FragColor = vec4(result, 1.0);
}