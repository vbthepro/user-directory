import { NavLink } from 'react-router-dom';
export function TopBar() { return <header className="topbar"><div className="brand">User Directory</div><nav><NavLink to="/">List</NavLink><NavLink to="/add">Add</NavLink></nav></header>; }
