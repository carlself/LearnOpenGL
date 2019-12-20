#version 330 core

layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in VS_OUT {
    vec2 texCoords;
} gs_in[];

out vec2 TexCoords;

uniform float time;

vec3 GetNormal()
{
    vec3 a = vec3(gl_in[0].gl_Position - gl_in[1].gl_Position);
    vec3 b = vec3(gl_in[2].gl_Position - gl_in[1].gl_Position);
    return normalize(cross(a, b));
}

float MAGNITUDE = 4.0;

vec4 Explode(vec4 position, vec3 normal)
{
    return position +  vec4(normal * ((sin(time) + 1.0)/2.0) * MAGNITUDE, 0.0);
}
void main()
{
    vec3 normal = GetNormal();

    gl_Position = Explode(gl_in[0].gl_Position, normal);
    TexCoords = gs_in[0].texCoords;
    EmitVertex();

    gl_Position = Explode(gl_in[1].gl_Position, normal);
    TexCoords = gs_in[1].texCoords;
    EmitVertex();

    gl_Position = Explode(gl_in[2].gl_Position, normal);
    TexCoords = gs_in[2].texCoords;
    EmitVertex();

    EndPrimitive();
}