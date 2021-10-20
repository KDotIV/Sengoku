import React from 'react';
import { Route } from 'react-router-dom';
//Componenets and pages
import Home from './pages/Home';
import Nav from './components/navComponent';
import PlayerCards from './pages/PlayerCards';
//Styling
import Pages from './components/Pages';

function App() {
  return (
    <div className="App">
      <Pages />
      <Nav />
      <Route path={["/event/:id", "/"]} exact component={Home} />
      <Route path={["/playercards/:id", "/playercards"]} exact component={PlayerCards}/>
    </div>
  );
}

export default App;
