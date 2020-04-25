import React, { Component } from 'react';
import FamilyTreeDiagram from './FamilyDiagram'

export default class FamilyTree extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            itemsData: [],
            title: null,
            loading: true
        };
    
    }

    componentDidMount() {
        this.populateItems();
    }

    render() {
        let contents = this.state.loading 
            ? <p><em>Loading...</em></p>
            : <FamilyTreeDiagram familyItems={this.state.itemsData} id={this.props.id} selectedPerson={this.props.selectedPerson}/>
        
        return (
            <div className="col-md-10">
                {contents}
            </div>
        );
    }

    async populateItems() {
        const response = await fetch('familytree');
        const data = await response.json();
        this.setState({ itemsData: data, loading: false });
    }
}