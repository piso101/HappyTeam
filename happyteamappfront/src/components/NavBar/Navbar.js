import React from 'react';
import Container from 'react-bootstrap/Container';

import Navbar from 'react-bootstrap/Navbar';
import { ReactComponent as Logo } from '../logo.svg';

function ColorSchemesExample() {
    return (
        <>
            <Navbar className="bg-cyan-700">
                <Container>
                    <Navbar.Brand href="#home" className="logo-container">
                        <Logo width="75" height="75" />
                    </Navbar.Brand>
                </Container>
            </Navbar>
        </>
    );
}

export default ColorSchemesExample;