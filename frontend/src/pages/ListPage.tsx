import { useEffect, useState } from 'react';
import { usersApi, type User } from '../api/users';
export function ListPage() {
  const [users, setUsers] = useState<User[]>([]); const [loading, setLoading] = useState(true); const [error, setError] = useState<string | null>(null);
  useEffect(() => { usersApi.list().then(setUsers).catch(e => setError(e instanceof Error ? e.message : 'Unable to load users')).finally(() => setLoading(false)); }, []);
  return <main className="container"><div className="page-head"><div><h1>Users</h1><p>Browse everyone in the directory.</p></div></div>{loading && <div className="state">Loading users…</div>}{error && !loading && <div className="alert error">Could not load users. {error}</div>}{!loading && !error && users.length === 0 && <div className="state">No users yet. Add the first user.</div>}{!loading && !error && users.length > 0 && <div className="table-wrap"><table><thead><tr><th>Name</th><th>Age</th><th>City</th><th>State</th><th>Pincode</th></tr></thead><tbody>{users.map(u => <tr key={u.id}><td>{u.name}</td><td>{u.age}</td><td>{u.city}</td><td>{u.state}</td><td>{u.pincode}</td></tr>)}</tbody></table></div>}</main>;
}
