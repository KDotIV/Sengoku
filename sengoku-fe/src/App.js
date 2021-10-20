import React from 'react';
//Componenets and pages
import Home from './pages/Home';
import PlayerCards from './pages/PlayerCards';
//Styling
import Pages from './components/Pages';

function App() {
  return (
    <div className="App">
      <Pages />
      <Home />
    </div>
  );
}

export default App;
