export type User = { id: string; name: string; age: number; city: string; state: string; pincode: string };
export type UserInput = Omit<User, 'id'>;
const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${baseUrl}${path}`, { headers: { 'Content-Type': 'application/json', ...(init?.headers || {}) }, ...init });
  if (!response.ok) throw new Error((await response.text()) || 'Request failed');
  return response.status === 204 ? (undefined as T) : response.json();
}

async function requestWithRetry<T>(path: string, attempts = 10): Promise<T> {
  for (let attempt = 1; attempt <= attempts; attempt++) {
    try {
      return await request<T>(path);
    } catch (error) {
      if (attempt === attempts || !(error instanceof TypeError)) throw error;
      await new Promise(resolve => setTimeout(resolve, 300));
    }
  }
  throw new Error('Request failed');
}

export const usersApi = {
  list: () => requestWithRetry<User[]>('/api/users'),
  create: (data: UserInput) => request<User>('/api/users', { method: 'POST', body: JSON.stringify(data) }),
};
