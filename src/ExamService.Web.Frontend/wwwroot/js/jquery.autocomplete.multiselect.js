
$.widget("ui.autocomplete", $.ui.autocomplete, {
    options: $.extend({}, this.options, {
        multiselect: false
    }),
    _create: function () {
        this._super();

        var self = this, o = self.options;

        if (o.multiselect) {
            //console.log('multiselect true');

            self.selectedItems = {};
            self.multiselect = $("<div></div>")
                .addClass("ui-autocomplete-multiselect ui-state-default ui-widget")
                .css("width", self.element.width())
                .insertBefore(self.element)
                .append(self.element)
                .bind("click.autocomplete", function () {
                    self.element.focus();

                    //alert("hello");
                    //this.search("a");
                    //$(this).data("autocomplete").search($(this).val());
                    $("#subjectAutocomplete").autocomplete("search", '');

                });

            var fontSize = parseInt(self.element.css("fontSize"), 10);
            function autoSize(e) {
                // Hackish autosizing
                var $this = $(this);
                $this.width(160).width(this.scrollWidth + fontSize - 1);
            };

            var kc = $.ui.keyCode;
            self.element.bind({
                "keydown.autocomplete": function (e) {
                    if ((this.value === "") && (e.keyCode == kc.BACKSPACE)) {
                        var prev = self.element.prev();
                        delete self.selectedItems[prev.text()];
                        prev.remove();
                    }
                },
                // TODO: Implement outline of container
                "focus.autocomplete blur.autocomplete": function () {
                    self.multiselect.toggleClass("ui-state-active");
                },
                "keypress.autocomplete change.autocomplete focus.autocomplete blur.autocomplete": autoSize
            }).trigger("change");

            self._renderItem = function (ul, item) {
                return $("<li>")
                  .append(
                    $("<div />")
                        .text(item.label)
                        .append(
                            $("<span />")
                                .addClass("badge label-info")
                                .text(item.count)
                        )
                   )
                  .appendTo(ul);
            };

            // TODO: There's a better way?
            o.select = o.select || function (e, ui) {
                $("<div></div>")
                    .addClass("ui-autocomplete-multiselect-item")
                    .text(ui.item.label)
                    .css("padding-bottom","5px")
                    .append(
                        $("<span></span>")
                            .addClass("badge label-info")
                            .text(ui.item.count)
                     )
                    .append(
                        $("<span></span>")
                            .addClass("ui-icon ui-icon-close")
                            .attr("id", JSON.stringify(ui.item))
                            //.attr("data-object", JSON.stringify(ui.item))
                            .click(function () {
                                var item = $(this).parent();
                                var raw_data = $(item, $("span")).context.id;
                                var json_data = $.parseJSON(raw_data);
                                subjectTags.push(json_data);

                                //var label = item.text();
                                //subjectTags.push(item.text());
                                //delete self.selectedItems[item.text()];
                                //delete self.selectedItems[ui.item.value];

                                delete self.selectedItems[json_data.value];
                                item.remove();

                                selectedSubjectItems = self.selectedItems;
                                /////////////
                                $("#checkboxItems input[type='checkbox']").removeAttr("checked");
                                GetQuestionsTypeCount();
                            })
                    )
                    .insertBefore(self.element);

                //self.selectedItems[ui.item.label] = ui.item;
                self.selectedItems[ui.item.value] = ui.item;
                self._value("");

                subjectTags = jQuery.grep(subjectTags, function (element) {
                    return element.value != ui.item.value;
                });
                $('#subjectAutocomplete').autocomplete('option', 'source', subjectTags)

                selectedSubjectItems = self.selectedItems;

                /////////////
                $("#checkboxItems input[type='checkbox']").removeAttr("checked");
                GetQuestionsTypeCount();

                //delete subjectTags[0];
                //console.log("self.selectedItems", self.selectedItems);
                
                //console.log($('#subjectAutocomplete').data()["ui-autocomplete"].selectedItems);
                //console.log("selectedSubjectItems", selectedSubjectItems);
                //console.log("subjectTags", subjectTags);
                //delete subjectTags[0];
                //console.log("subjectTags.indexOf( item )", subjectTags.indexOf(ui.item));

                //var index = subjectTags.indexOf(ui.item.label);
                //subjectTags.remove(index);
                //delete subjectTags[index];

                //subjectTags = jQuery.grep(subjectTags, function (element) {
                //    return element.value != ui.item.value;
                //});
                //$('#subjectAutocomplete').autocomplete('option', 'source', subjectTags)

               // subjectTags.splice($.inArray(ui.item.value, subjectTags), 1);
                //console.log(subjectTags);

                return false;
            }

            /*
            self.options.open = function(e, ui) {
                var pos = self.multiselect.position();
                pos.top += self.multiselect.height();
                self.menu.element.position(pos);
            }
            */
        }

        return this;
    }
});