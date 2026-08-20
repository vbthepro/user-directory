import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { useState } from 'react';
import { TopBar } from './components/TopBar';
import { Toast } from './components/Toast';
import { ListPage } from './pages/ListPage';
import { AddPage } from './pages/AddPage';
import './styles.css';
export default function App() { const [toast,setToast] = useState<string|null>(null); const authEnabled = import.meta.env.VITE_AUTH_ENABLED === 'true'; return <BrowserRouter><TopBar/><Routes><Route path="/" element={<ListPage/>}/><Route path="/add" element={authEnabled ? <div className="container"><div className="state">Configure your OIDC client before enabling the protected Add page.</div></div> : <AddPage onSuccess={setToast}/>}/></Routes><Toast message={toast}/></BrowserRouter>; }
