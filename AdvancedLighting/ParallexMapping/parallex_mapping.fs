#version 330 core
out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec2 TexCoords;
    vec3 TangentLightPos;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
} fs_in;

uniform sampler2D diffuseMap;
uniform sampler2D normalMap;
uniform sampler2D depthMap;
uniform float height_scale;

vec2 ParallexMapping(vec2 texCoords, vec3 viewDir)
{
    float height = texture(depthMap, texCoords).r;
    vec2 p = viewDir.xy * (height * height_scale);
    return texCoords - p;
}


void main()
{
    vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.TangentViewPos);

    vec2 texCoords = ParallexMapping(fs_in.TexCoords, viewDir);
    // if(texCoords.x > 1.0 || texCoords.y > 1.0 || texCoords.x < 0.0 || texCoords.y < 0.0)
    //     discard;

    vec3 texColor = texture(diffuseMap, texCoords).rgb;
    vec3 normal = texture(normalMap, texCoords).rgb;
    normal = normalize(normal * 2.0 - 1.0);

    vec3 ambient = 0.1 * texColor;

    vec3 lightDir = normalize(fs_in.TangentLightPos - fs_in.TangentFragPos);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = diff * texColor;

    vec3 halfDir = normalize(viewDir + lightDir);
    float spec = pow(max(dot(halfDir, normal), 0.0),64.0);
    vec3 specular = spec * vec3(0.2);

    FragColor = vec4(ambient + diffuse + specular, 1.0);
}