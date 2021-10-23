import React from "react";
import styled from "styled-components";
import { motion } from "framer-motion";
//Redux API
import { useDispatch } from 'react-redux';
import { NavLink as Link} from 'react-router-dom';

const Nav = () => {
    const dispatch = useDispatch();

    return(
    <>
        <StyleNav>
            <NavMenu>
                <NavLink to="/playercards">
                    Players
                </NavLink>
                <NavLink to="/events">
                    Events
                </NavLink>
                <NavLink to="/legends">
                    Legends
                </NavLink>
                <NavBtn>
                    <NavBtnLink to="/login">Sign In</NavBtnLink>
                </NavBtn>
            </NavMenu>
        </StyleNav>
        <HeaderStyles>
            <Logo>
                <Link to="/">Sengoku</Link>
            </Logo>
            <input type="text"/>
            <button>Search</button>
        </HeaderStyles>
    </>
    );
}

const StyleNav = styled(motion.nav)`
    height: 80px;
    display: flex;
    justify-content: space-between;
    padding: 3rem 0.1rem;
    z-index: 10;
`;

const NavLink = styled(Link)`
    display: flex;
    align-items: center;
    text-decoration: none;
    padding: 0rem 2rem;
    height: 100%;
    cursor: pointer;
`;

const NavMenu = styled.div`
    display: flex;
    align-items: center;
    margin-right: -24px;

    @media screen and (max-width: 768px) {
        display: none;
    }
`;

const NavBtn = styled.nav`
    display: flex;
    align-items: center;
    margin-right: 24px;

    @media screen and (max-width: 768px) {
        display: none;
    }
`;

const NavBtnLink = styled(Link)`
    border-radius: 4px;
`;

const Logo = styled.h2`
    height: auto;
    font-size: 2rem;
    position: relative;
    background: LightCoral;
    overflow: hidden;
    p {
        color: white;
        text-align: center;
        text-decoration: none;
        padding: 0.5rem 1rem;
    }
`;

const HeaderStyles = styled(motion.div)`
    padding: 1rem 0.1rem;
    text-align: center;
    input{
        width: 30%;
        font-size: 1.5rem;
        padding: 0.5rem;
        border: none;
        margin-top: 1rem;
        box-shadow: 0px 0px 30px rgba(0,0,0,0.2);
    }
    button{
        font-size: 1.5rem;
        border: none;
        padding: 0.5rem 2rem;
        cursor: pointer;
        background: #ff7676;
        color: white;
    }
`;
export default Nav;