import React from 'react';
import { Route } from 'react-router-dom';
//Componenets and pages
import Home from './pages/Home';
import Events from './pages/Events';
import Legends from './pages/Legends';
import Nav from './components/navComponent';
import PlayerCards from './pages/PlayerCards';
//Styling
import Pages from './components/Pages';

function App() {
  return (
    <div className="App">
      <Pages />
      <Nav />
      <Route path={["/"]} exact component={Home} />
      <Route path={["/playercards/:id", "/playercards"]} exact component={PlayerCards}/>
      <Route path={["/events/:id", "/events"]} exact component={Events} />
      <Route path={["/legends/:id", "/legends"]} exact component={Legends} />
    </div>
  );
}

export default App;
