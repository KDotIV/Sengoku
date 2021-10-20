import React, { useEffect } from 'react';
import { useDispatch, useSelector } from "react-redux";
//Components
import Players from "../components/playerComponent";
import Pages from "../components/Pages";
//Styling
import styled from 'styled-components';
import { motion } from "framer-motion";
import { loadPlayers } from '../actions/playersAction';

const PlayerCards = () => {
    const dispatch = useDispatch();
    useEffect(() => {
      dispatch(loadPlayers());
    }, [dispatch]);

    const {allPlayers} = useSelector((state) => state.players);
    return (
    <PlayerList>
        <h2>PlayerCards</h2>
        <PlayerStyles>
            {allPlayers.map((result) => (
                <Players
                id={result.playerId}
                playerName={result.name}
                key={result.playerId}
                />
            ))}
        </PlayerStyles>
    </PlayerList>
    );
}

const PlayerList = styled(motion.div)`
    padding: 0rem 5rem;
    h2{
        padding: 5rem 0rem;
    }
`;

const PlayerStyles = styled(motion.div)`
    min-height: 80vh;
    display: grid;
    grid-template-columns: repeat(auto-fit,minmax(500px, 1fr));
    grid-column-gap: 3rem;
    grid-row-gap: 5rem;
`;

const Logo = styled.h1`
    height: auto;
    font-size: 2rem;
    margin-left: 1rem;
    position: relative;
    z-index: 2;
    background: LightCoral;
    overflow: hidden;
    p {
        color: white;
        text-align: center;
        text-decoration: none;
        padding: 0.5rem 1rem;
    }
`;

const HeaderStyles = styled.header`
  .bar {
    border-bottom: 10px solid var(--black, black);
    display: grid;
    grid-template-columns: auto 1fr;
    justify-content: space-between;
    align-items: center;
  }

  .sub-bar {
    display: grid;
    grid-template-columns: 1fr auto;
    border-bottom: 1px solid var(--black, black);
    h3 {
        margin-left: 1rem;
    }
  }
`;

export default PlayerCards;