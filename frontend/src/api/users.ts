export type User = { id: string; name: string; age: number; city: string; state: string; pincode: string };
export type UserInput = Omit<User, 'id'>;
const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${baseUrl}${path}`, { headers: { 'Content-Type': 'application/json', ...(init?.headers || {}) }, ...init });
  if (!response.ok) throw new Error((await response.text()) || 'Request failed');
  return response.status === 204 ? (undefined as T) : response.json();
}
export const usersApi = {
  list: () => request<User[]>('/api/users'),
  create: (data: UserInput) => request<User>('/api/users', { method: 'POST', body: JSON.stringify(data) }),
};
