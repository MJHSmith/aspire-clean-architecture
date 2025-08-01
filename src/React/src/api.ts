const API_BASE = import.meta.env["VITE_API_BASE_HTTPS"] || "";
let token: string | null = null;

export async function login(email: string, password: string) {
    console.log("API_BASE=" + API_BASE);
    const res = await fetch(`${API_BASE}/users/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });
  if (!res.ok) throw new Error("Login failed");
    token = JSON.parse(await res.text());
}

export function getToken() {
  return token;
}

export async function fetchAnimals() {
    const res = await fetch(`${API_BASE}/animals`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!res.ok) throw new Error("Failed to fetch animals");
  return res.json();
}

export async function deleteAnimal(type: string, id: string) {
    const res = await fetch(`${API_BASE}/animal/${type}/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!res.ok) throw new Error("Delete failed");
}

export async function createAnimal(type: string, data: any) {
  let url = "";
  let body: any = {};
  if (type === "Cat") {
    url = "/cat";
    body = { Name: data.name, HairColourDescription: data.hairColourDescription };
  } else if (type === "Dog") {
    url = "/dog";
    body = {
      Name: data.name,
      HairColour1: data.hairColour1,
      HairColour2: data.hairColour2,
      HairPattern: data.hairPattern,
    };
  } else if (type === "Bird") {
    url = "/bird";
    body = { Name: data.name, WingspanInCentimeters: Number(data.wingspanInCentimeters) };
  }
  const res = await fetch(API_BASE + url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error("Create failed");
}

export async function fetchAnimal(type: string, id: string) {
  const res = await fetch(`${API_BASE}/animal/${type}/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!res.ok) throw new Error("Failed to fetch animal");
  return res.json();
}