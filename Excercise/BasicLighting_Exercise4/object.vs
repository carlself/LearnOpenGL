#version 330 core 

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec3 lightColor;
uniform vec3 objectColor;

uniform vec3 lightPos;
uniform vec3 viewPos;

out vec3 resultColor;

void main()
{
   gl_Position = projection * view * model * vec4(aPos, 1.0);

   vec3 normal  = mat3(transpose(inverse(model))) * aNormal; // should be done in cpu
   normal = normalize(normal);

   float ambientStrength = 0.1;
   vec3 ambient = ambientStrength * lightColor;

   vec3 worldPos = vec3(model * vec4(aPos, 1.0));
   vec3 lightDir = normalize(lightPos - worldPos);

   vec3 diffuse = max(dot(lightDir, normal), 0) * lightColor;

   vec3 viewDir = normalize(viewPos - worldPos);
   vec3 reflectDir = reflect(-lightDir, normal);

   float specularStrength = 1.0;

   vec3 specular = pow(max(dot(viewDir, reflectDir), 0.0), 32) * specularStrength * lightColor;

   resultColor = (ambient + diffuse + specular) * objectColor;
}