import { useEffect, useState } from 'react'

import { fetchAnimals } from "./api";
import { LoginForm } from "./components/LoginForm";
import { AnimalTable } from "./components/AnimalTable";
import { AnimalForm } from "./components/AnimalForm";

import './App.css'

function App() {
    const [loggedIn, setLoggedIn] = useState(false);
    const [animals, setAnimals] = useState<any[]>([]);

    const loadAnimals = () => fetchAnimals().then(setAnimals);

    useEffect(() => {
        if (loggedIn) loadAnimals();
    }, [loggedIn]);

    if (!loggedIn) return <LoginForm onLogin={() => setLoggedIn(true)} />;

  return (
    <>
      <main>
        <h1>Animals</h1>
        <AnimalForm onCreated={loadAnimals} />
        <AnimalTable animals={animals} onDelete={loadAnimals} />
      </main>
    </>
  )
}

export default App
