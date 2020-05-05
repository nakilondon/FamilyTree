import React, { Component } from 'react';
import { FamDiagram } from 'basicprimitivesreact';
import primitives from 'basicprimitives';
import './FamilyTree.css'

export default class FamilyTreeDiagram extends Component {
    constructor(props) {
        super(props);
        this.onCursorChanged = this.onCursorChanged.bind(this);
    };

    onCursorChanged(event, data) {
        const { context: item } = data;
        if (item != null) {
           this.props.selectedPerson(item.id);
        };
    };

    renderDiagram(items, CursorChange, id) {
        let config = {
            cursorItem: id,
            pageFitMode: primitives.common.PageFitMode.AutoSize,
            autoSizeMinimum: { width: 100, height: 100 },
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
        return this.renderDiagram(this.props.familyItems, this.onCursorChanged, this.props.id);
    }

}