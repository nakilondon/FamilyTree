import React, { Component } from 'react';
import { FamDiagram, OrgDiagramItemConfig } from 'basicprimitivesreact';
import primitives from 'basicprimitives';
import './FamilyTree.css'

export default class FamilyTree extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            itemsData: [],
            title: null,
            loading: true
        };
    
        this.onCursorChanged = this.onCursorChanged.bind(this);
    }

    onCursorChanged(event, data) {
        const { context: item } = data;
        if (item != null) {
           this.props.selectedPerson(item.id);
        };
      };

    componentDidMount() {
        this.populateItems();
    }

    static renderDiagram(items, CursorChange, id) {
        let config = {
            cursorItem: id,
            neighboursSelectionMode: primitives.common.NeighboursSelectionMode.ParentsChildrenSiblingsAndSpouses,
            hasSelectorCheckbox: primitives.common.Enabled.False,
            normalLevelShift: 20,
            dotLevelShift: 20,
            lineLevelShift: 10,
            normalItemsInterval: 10,
            dotItemsInterval: 10,
            lineItemsInterval: 4,
            items: items
          };

        return <div className="FamilyTree">
            <FamDiagram centerOnCursor={true} onCursorChanged={CursorChange} config={config} />
        </div>
    }

    render() {
        let contents = this.state.loading 
            ? <p><em>Loading...</em></p>
            : FamilyTree.renderDiagram(this.state.itemsData, this.onCursorChanged, this.props.id);
        
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