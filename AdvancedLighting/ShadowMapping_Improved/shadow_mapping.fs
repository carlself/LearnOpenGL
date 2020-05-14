#version 330 core

out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
} fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

float ShadowCalculation(vec4 fragPosLightSpace, vec3 normal, vec3 lightDir)
{
    vec3 projCoords = fs_in.FragPosLightSpace.xyz / fs_in.FragPosLightSpace.w; // [-1,1]
    projCoords = projCoords * 0.5 + 0.5; // [0,1]
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    
    float bias = max(0.05* (1.0 - dot(normal, lightDir)), 0.005);
    if(projCoords.z > 1.0)
    {
        shadow = 0.0;
    }
    else
    {
        // float closestDepth = texture(shadowMap, projCoords.xy).r;
        float currentDepth = projCoords.z;

        for(int i = -2; i <=2; i++)
        {
            for(int j = -2; j <= 2; j++)
            {
                float depth = texture(shadowMap, projCoords.xy + vec2(i,j) * texelSize).r;
                shadow += currentDepth - bias > depth ? 1.0 : 0.0;
            }
        }

        shadow = shadow/9.0;
    }

    return shadow;
}

void main()
{
    vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
    vec3 normal = normalize(fs_in.Normal);
    vec3 lightColor = vec3(1.0);

    vec3 ambient = 0.15 * lightColor;
    
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    vec3 halfDir = normalize(viewDir + lightDir);
    float spec = pow(max(dot(halfDir, normal), 0.0), 64.0);
    vec3 specular = spec * lightColor;

    float shadow = ShadowCalculation(normal, lightDir);
    vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular)) * color;

    FragColor = vec4(lighting, 1.0);
}