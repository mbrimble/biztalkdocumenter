
using Microsoft.Services.Tools.BizTalkOM.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM
{
    using System;
    using System.Collections;
    using System.IO;
    using Microsoft.RuleEngine;

    /// <summary>
    /// Summary description for RuleSetWriter.
    /// </summary>
    public class RuleSetWriter
    {
        private Hashtable vdefs = new Hashtable();
        private StreamWriter writer = null;

        public RuleSetWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        public Hashtable VocabularyDefinitions
        {
            set { this.vdefs = value; }
        }

        public void WriteRuleSet(RuleSet ruleSet, VocabularyInfoCollection referencedVocabs, StreamWriter writer)
        {
            SortedList sl = new SortedList();

            this.WriteStartOfPage(ruleSet);

            this.WriteReferencedVocabs(referencedVocabs);

            foreach (Rule rule in ruleSet.Rules.Values)
            {
                sl.Add(rule.Name + "|" + rule.GetHashCode(), rule);
            }

            int counter = 0;

            foreach (Rule rule in sl.Values)
            {
                WriteRule(rule, counter);
                this.WriteRuleSeparator();
                counter++;
            }

            this.WriteEndOfPage();

            this.writer.Flush();

            return;
        }

        private void WriteReferencedVocabs(VocabularyInfoCollection referencedVocabs)
        {
            if (referencedVocabs != null && referencedVocabs.Count > 0)
            {
                this.writer.WriteLine("	<table class='TableData' ID='Table3'>");
                this.writer.WriteLine("		<tr>");
                this.writer.WriteLine("			<td width='10'></td>");
                this.writer.WriteLine("			<td class='TableTitle'>Referenced Vocabularies:</td>");
                this.writer.WriteLine("		</tr>");

                foreach (VocabularyInfo vi in referencedVocabs)
                {
                    this.writer.WriteLine("		<tr>");
                    this.writer.WriteLine("			<td width='10'></td>");
                    this.writer.WriteLine("			<td class='TableData'><a class='TableData' href='../Vocabulary/{0} {1}.{2}.html'>{0} {1}.{2}</a></td>", vi.Name, vi.MajorRevision, vi.MinorRevision);
                    this.writer.WriteLine("		</tr>");
                }

                this.writer.WriteLine("	</table>");
            }
        }

        #region Helpers

        private void WriteStartOfPage(RuleSet ruleSet)
        {
            this.writer.WriteLine("<html>");
            this.writer.WriteLine("<HEAD>");
            this.writer.WriteLine("	<META http-equiv='Content-Type' content='text/html; charset=utf-8'>");
            this.writer.WriteLine("	<link href='../CommentReport.css' type='text/css' rel='stylesheet'>");

            this.writer.WriteLine("	<script language='javaScript'>");
            this.writer.WriteLine("		function ShowItem( itemName )");
            this.writer.WriteLine("		{");
            this.writer.WriteLine("			eval( 'var itemStyle = ' + itemName + '.style' )");
            this.writer.WriteLine("			if( itemStyle.display=='none')");
            this.writer.WriteLine("				itemStyle.display='block';");
            this.writer.WriteLine("			else");
            this.writer.WriteLine("				itemStyle.display='none';");
            this.writer.WriteLine("		}");
            this.writer.WriteLine("	</script>");

            this.writer.WriteLine("</HEAD>");
            this.writer.WriteLine("<body>");
            this.writer.WriteLine("	<SPAN CLASS='StartOfFile'>Policy Reference</SPAN><BR>");
            this.writer.WriteLine("	<BR>");
            this.writer.WriteLine("	<table ID='Table1'>");
            this.writer.WriteLine("		<tr>");
            this.writer.WriteLine("			<td width='10'></td>");
            this.writer.WriteLine("			<td valign='center' CLASS='PageTitle'>Policy : {0}</td>", ruleSet.Name);
            this.writer.WriteLine("		</tr>");
            this.writer.WriteLine("	</table>");
            this.writer.WriteLine("	<BR>");
            this.writer.WriteLine("	<table class='TableData' ID='Table2'>");
            this.writer.WriteLine("		<tr>");
            this.writer.WriteLine("			<td width='10'></td>");
            this.writer.WriteLine("			<td width='145' class='TableTitle'>Description:</td>");
            this.writer.WriteLine("			<td class='TableData'>{0}</td>", ruleSet.CurrentVersion.Description.Length > 0 ? ruleSet.CurrentVersion.Description : "N/A");
            this.writer.WriteLine("		</tr>");
            this.writer.WriteLine("		<tr>");
            this.writer.WriteLine("			<td width='10'></td>");
            this.writer.WriteLine("			<td width='145' class='TableTitle'>Version:</td>");
            this.writer.WriteLine("			<td class='TableData'>{0}.{1}</td>", ruleSet.CurrentVersion.MajorRevision, ruleSet.CurrentVersion.MinorRevision);
            this.writer.WriteLine("		</tr>");
            this.writer.WriteLine("		<tr>");
            this.writer.WriteLine("			<td width='10'></td>");
            this.writer.WriteLine("			<td width='145' class='TableTitle'>Last Modified:</td>");
            this.writer.WriteLine("			<td class='TableData'>{0} ({1})</td>", ruleSet.CurrentVersion.ModifiedTime, ruleSet.CurrentVersion.ModifiedBy);
            this.writer.WriteLine("		</tr>");
            this.writer.WriteLine("		<tr>");
            this.writer.WriteLine("			<td width='10'></td>");
            this.writer.WriteLine("			<td width='145' class='TableTitle'><a class='TableData' href='{0} {1}.{2}.xml'>Policy Source File</a></td>", ruleSet.Name, ruleSet.CurrentVersion.MajorRevision, ruleSet.CurrentVersion.MinorRevision);
            this.writer.WriteLine("			<td class='TableData'></td>");
            this.writer.WriteLine("		</tr>");

            this.writer.WriteLine("	</table>");
            this.writer.WriteLine("	<BR>");
        }

        private void WriteEndOfPage()
        {
            this.writer.WriteLine("<BR></body></html>");
        }

        private void BeginList()
        {
            this.writer.WriteLine("<UL>");
        }

        private void EndList()
        {
            this.writer.WriteLine("</UL>");
        }

        private void BeginListItem()
        {
            this.writer.WriteLine(@"<LI type='square'>");
        }

        private void EndListItem()
        {
            this.writer.WriteLine("</LI>");
        }

        private void WriteRuleSeparator()
        {
            this.writer.WriteLine("");
        }

        #endregion

        private void WriteRule(Rule rule, int counter)
        {
            this.BeginList();

            this.BeginListItem();
            this.writer.WriteLine("<span class='TableTitle' style='cursor: hand;'><img src='RuleRule.jpg'> <u><a onClick=\"ShowItem('Item{1}');\">Rule: {0}</a></u></span>", rule.Name, counter);
            this.EndListItem();

            this.writer.WriteLine("<div id='Item{0}' style='display: none;'>", counter);
            this.BeginList();
            this.WriteRuleConditions(rule.Conditions);
            this.WriteRuleActions(rule.Actions);
            this.EndList();
            this.writer.WriteLine("</div>");

            this.EndList();
        }

        private void WriteRuleConditions(LogicalExpression conditions)
        {
            this.BeginListItem();
            this.writer.WriteLine("<img src='RuleIf.jpg'><SPAN class='TableTitle'>IF</SPAN>");
            this.WriteCondition(conditions);
            this.EndListItem();
        }

        private void WriteRuleActions(ActionCollection actions)
        {
            this.BeginListItem();
            this.writer.WriteLine("<img src='RuleThen.jpg'><SPAN class='TableTitle'>THEN</SPAN>");

            this.BeginList();

            foreach (Function action in actions)
            {
                this.BeginListItem();
                this.writer.WriteLine(this.WriteTerm(action));
                this.EndListItem();
            }

            this.EndList();
            this.EndListItem();
        }

        private string GetVocabDisplayString(string definitionId)
        {
            if (vdefs.ContainsKey(definitionId))
            {
                VocabularyDefinition vd = vdefs[definitionId] as VocabularyDefinition;
                return vd.GetFormatString().Format;
            }
            else
            {
                return "";
            }
        }

        #region WriteTerm

        private string WriteTerm(Term term)
        {
            string displayString = "";
            ArgumentCollection args = null;

            if (term is Constant)
            {
                if (((Constant)term).Value != null)
                {
                    if (((Constant)term).Value.ToString() == string.Empty)
                    {
                        displayString = "&lt;empty string&gt;";
                    }
                    else
                    {
                        displayString = ((Constant)term).Value.ToString();
                    }
                }
                else
                {
                    displayString = "&lt;null&gt;";
                }
            }

            else if (term is UserFunction || term is ObjectReference)
            {
                Binding termBinding = null;

                if (term is UserFunction)
                {
                    termBinding = ((UserFunction)term).Binding;
                }
                else
                {
                    termBinding = ((ObjectReference)term).Binding;
                }

                if (termBinding is XMLDocumentBinding)
                {
                    XMLDocumentBinding binding = termBinding as XMLDocumentBinding;
                    displayString = binding.DocumentType;
                }
                else if (termBinding is DatabaseBinding)
                {
                    DatabaseBinding binding = termBinding as DatabaseBinding;
                    displayString = binding.DatasetName;
                }
                else if (termBinding is ClassBinding)
                {
                    ClassBinding binding = termBinding as ClassBinding;
                    displayString = binding.TypeName;
                }
                else if (termBinding is XMLDocumentFieldBinding)
                {
                    XMLDocumentFieldBinding binding = termBinding as XMLDocumentFieldBinding;
                    displayString = binding.DocumentBinding.DocumentType + ":" + binding.DocumentBinding.Selector.Alias + "/" + binding.Field.Alias;

                    if (binding.Argument != null)
                    {
                        if (binding.MemberName.ToLower() == "setstring")
                        {
                            displayString += " = {0}";
                        }

                        args = new ArgumentCollection();
                        args.Add(binding.Argument);
                    }
                }
                else if (termBinding is DatabaseColumnBinding)
                {
                    DatabaseColumnBinding binding = termBinding as DatabaseColumnBinding;
                    displayString = binding.DatabaseBinding.DatasetName + "." + binding.DatabaseBinding.TableName + "." + binding.ColumnName;

                    if (binding.Argument != null)
                    {
                        args = new ArgumentCollection();
                        args.Add(binding.Argument);
                    }
                }
                else if (termBinding is DataRowBinding)
                {
                    DataRowBinding binding = termBinding as DataRowBinding;
                    displayString = binding.DisplayName;
                }
                else if (termBinding is ClassMemberBinding)
                {
                    ClassMemberBinding binding = termBinding as ClassMemberBinding;
                    displayString = binding.ClassBinding.ImplementingType.FullName + "." + binding.MemberName;
                    args = binding.Arguments;
                }
            }
            else if (term is ArithmeticFunction)
            {
                ArithmeticFunction f = term as ArithmeticFunction;
                args = new ArgumentCollection();
                args.Add(f.LeftArgument);
                args.Add(f.RightArgument);
            }
            else if (term is Assert)
            {
                args = new ArgumentCollection();
                args.Add(((Assert)term).Facts);
            }
            else if (term is Retract)
            {
                args = new ArgumentCollection();
                args.Add(((Retract)term).Facts);
            }
            else if (term is Update)
            {
                args = new ArgumentCollection();
                args.Add(((Update)term).Facts);
            }
            else if (term is Halt)
            {
                args = new ArgumentCollection();
                args.Add(((Halt)term).ClearAgenda);
            }

            ArrayList argsToDisplay = new ArrayList();

            if (args != null)
            {
                foreach (Term t in args)
                {
                    argsToDisplay.Add(WriteTerm(t));
                }
            }

            string format = "";
            if (term.VocabularyLink != null)
            {
                VocabularyDefinition vd = vdefs[term.VocabularyLink.DefinitionId] as VocabularyDefinition;
                format = vd.GetFormatString().Format;

                // Added to fix halt documentation CD 20140328
                if (format == "halt choices")
                {
                    format = "{0}";
                    if (displayString == "True")
                    {
                        argsToDisplay.Add("clear all rules firings");
                    }
                    else
                    {
                        argsToDisplay.Add("do not clear");
                    }
                }

            }
            else
            {
                format = displayString;
            }

            displayString = TryStringFormat(format, argsToDisplay.ToArray());

            return "<span class='TableData'>" + displayString + "</span>";
        }

        #endregion

        private string GetFakePredicateDisplayString(LogicalExpression predicate)
        {
            string displayString = "{0}";

            if (predicate is Equal) displayString = "{0} is equal to {1}";
            if (predicate is GreaterThan) displayString = "{0} is greater than {1}";
            if (predicate is GreaterThanEqual) displayString = "{0} is greater than or equal to {1}";
            if (predicate is LessThan) displayString = "{0} is less than {1}";
            if (predicate is LessThanEqual) displayString = "{0} is less than or equal to {1}";
            if (predicate is NotEqual) displayString = "{0} is not equal to {1}";

            if (predicate is After) displayString = "{0} is after {1}";
            if (predicate is Before) displayString = "{0} is before {1}";
            if (predicate is Between) displayString = "{0} is between {1} and {2}";
            if (predicate is Exists) displayString = "{1} exists in {0}";
            if (predicate is Match) displayString = "{1} contains {0}";
            if (predicate is Range) displayString = "{0} is between {1} and {2}";

            return displayString;
        }

        #region WriteCondition

        private void WriteCondition(LogicalExpression condition)
        {
            ArrayList args = new ArrayList();

            string format = "";
            bool isLogical = false;

            if (condition.VocabularyLink != null)
            {
                format = GetVocabDisplayString(condition.VocabularyLink.DefinitionId);
            }

            //======================================
            // AND
            //======================================
            if (condition is LogicalAnd)
            {
                isLogical = true;

                this.BeginList();

                this.BeginListItem();
                this.writer.WriteLine("<img src='RuleAnd.jpg'><SPAN class='TableTitle'>AND</SPAN>");
                this.EndListItem();

                foreach (LogicalExpression le in ((LogicalAnd)condition).Arguments)
                {
                    WriteCondition(le);
                }
                this.EndList();
            }
            //======================================
            // OR
            //======================================
            else if (condition is LogicalOr)
            {
                isLogical = true;

                this.BeginList();

                this.BeginListItem();
                this.writer.WriteLine("<img src='RuleOr.jpg'><SPAN class='TableTitle'>OR</SPAN>");
                this.EndListItem();

                foreach (LogicalExpression le in ((LogicalOr)condition).Arguments)
                {
                    WriteCondition(le);
                }
                this.EndList();
            }
            //======================================
            // NOT
            //======================================
            else if (condition is LogicalNot)
            {
                isLogical = true;

                this.BeginList();

                this.BeginListItem();
                this.writer.WriteLine("<img src='RuleNot.jpg'><SPAN class='TableTitle'>NOT</SPAN>");
                this.EndListItem();

                WriteCondition(((LogicalNot)condition).Argument);
                this.EndList();
            }

            if (!isLogical)
            {
                this.BeginList();
                this.BeginListItem();

                if (condition.VocabularyLink == null)
                {
                    format = GetFakePredicateDisplayString(condition);
                }
            }

            //======================================
            // RELATIONAL PREDICATE
            //======================================
            if (condition is RelationalPredicate)
            {
                RelationalPredicate predicate = condition as RelationalPredicate;

                string a1 = WriteTerm(predicate.LeftArgument);
                string a2 = WriteTerm(predicate.RightArgument);

                args.Add(a1);
                args.Add(a2);
            }

            //======================================
            // BEFORE
            //======================================
            if (condition is Before)
            {
                Before before = condition as Before;

                string a1 = WriteTerm(before.Time1);
                string a2 = WriteTerm(before.Time2);

                //if (before.Time1.VocabularyLink != null)
                //{
                //    a1 = GetVocabDisplayString(before.Time1.VocabularyLink.DefinitionId);
                //}

                //if (before.Time2.VocabularyLink != null)
                //{
                //    a2 = GetVocabDisplayString(before.Time2.VocabularyLink.DefinitionId);
                //} //CD 2014/03/04

                args.Add(a1);
                args.Add(a2);
            }

            if (condition is After)
            {
                After after = condition as After;

                string a1 = WriteTerm(after.Time1);
                string a2 = WriteTerm(after.Time2);

                //if (after.Time1.VocabularyLink != null)
                //{
                //    a1 = GetVocabDisplayString(after.Time1.VocabularyLink.DefinitionId);
                //}

                //if (after.Time2.VocabularyLink != null)
                //{
                //    a2 = GetVocabDisplayString(after.Time2.VocabularyLink.DefinitionId);
                //} //CD 2014/03/04

                args.Add(a1);
                args.Add(a2);
            }

            if (condition is Between)
            {
                Between between = condition as Between;

                string a1 = WriteTerm(between.Time1);
                string a2 = WriteTerm(between.Time2);
                string a3 = WriteTerm(between.Time3);

                if (between.Time1.VocabularyLink != null)
                {
                    a1 = GetVocabDisplayString(between.Time1.VocabularyLink.DefinitionId);
                }

                //if (between.Time2.VocabularyLink != null)
                //{
                //    a2 = GetVocabDisplayString(between.Time2.VocabularyLink.DefinitionId);
                //}

                //if (between.Time3.VocabularyLink != null)
                //{
                //    a3 = GetVocabDisplayString(between.Time3.VocabularyLink.DefinitionId);
                //} CD 2014/03/04

                args.Add(a1);
                args.Add(a2);
                args.Add(a3);
            }

            if (condition is Range)
            {
                Range range = condition as Range;

                string a1 = WriteTerm(range.TestValue);
                string a2 = WriteTerm(range.RangeLow);
                string a3 = WriteTerm(range.RangeHigh);

                //if (range.TestValue.VocabularyLink != null)
                //{
                //    a1 = GetVocabDisplayString(range.TestValue.VocabularyLink.DefinitionId);
                //}

                //if (range.RangeLow.VocabularyLink != null)
                //{
                //    a2 = GetVocabDisplayString(range.RangeLow.VocabularyLink.DefinitionId);
                //}

                //if (range.RangeHigh.VocabularyLink != null)
                //{
                //    a3 = GetVocabDisplayString(range.RangeHigh.VocabularyLink.DefinitionId);
                //} CD 2014/03/04

                args.Add(a1);
                args.Add(a2);
                args.Add(a3);
            }

            if (condition is Match)
            {
                Match match = condition as Match;

                string a1 = WriteTerm(match.RegularExpression);
                string a2 = WriteTerm(match.InputString);

                // Does not appear to be required MTB 2014/02/10
                //if (match.InputString.VocabularyLink != null)
                //{
                //    //a1 = GetVocabDisplayString(match.InputString.VocabularyLink.DefinitionId); MTB 6/02/2014 Match contains predicate bug
                //    a2 = GetVocabDisplayString(match.InputString.VocabularyLink.DefinitionId);
                //}

                //if (match.RegularExpression.VocabularyLink != null)
                //{
                //    //a2 = GetVocabDisplayString(match.RegularExpression.VocabularyLink.DefinitionId); MTB 6/02/2014 Match contains bug
                //    a1 = GetVocabDisplayString(match.RegularExpression.VocabularyLink.DefinitionId);
                //}

                args.Add(a1);
                args.Add(a2);
            }

            if (condition is Exists)
            {
                Exists exists = condition as Exists;

                string a1 = WriteTerm(exists.ObjReference);
                string a2 = WriteTerm(exists.MemberName);

                if (exists.ObjReference.VocabularyLink != null)
                {
                    a1 = GetVocabDisplayString(exists.ObjReference.VocabularyLink.DefinitionId);
                }

                if (exists.MemberName.VocabularyLink != null)
                {
                    a2 = GetVocabDisplayString(exists.MemberName.VocabularyLink.DefinitionId);
                }

                args.Add(a1);
                args.Add(a2);
            }

            if (args.Count > 0)
            {
                if (format.Length == 0)
                {
                    int argCounter = 0;
                    foreach (string arg in args)
                    {
                        format += "{" + argCounter.ToString() + "} ";
                        break;
                    }
                }

                this.writer.WriteLine("<span class='TableData'><b>{0}</b></span>", TryStringFormat(format, args.ToArray()));
            }
            else
            {
                this.writer.WriteLine("<span class='TableData'>{0}</span>", format);
            }

            //this.writer.WriteLine("<span class='TableData'>{0}</span>", format);

            if (!isLogical)
            {
                this.EndList();
                this.EndListItem();
            }
        }

        #endregion

        #region TryStringFormat

        private string TryStringFormat(string format, params object[] args)
        {
            try
            {
                return string.Format(format, args);
            }
            catch (Exception ex)
            {
                TraceManager.SmartTrace.TraceError(ex);
                return format;
            }
        }

        #endregion
    }
}
