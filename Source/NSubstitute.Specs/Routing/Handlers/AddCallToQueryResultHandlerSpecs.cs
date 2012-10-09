﻿using NSubstitute.Core;
using NSubstitute.Routing.Handlers;
using NSubstitute.Specs.Infrastructure;
using NUnit.Framework;

namespace NSubstitute.Specs.Routing.Handlers
{
    public class AddCallToQueryResultHandlerSpecs
    {
        public class When_handling_call : ConcernFor<AddCallToQueryResultHandler>
        {
            private readonly object _target = new object();
            private ICall _call;
            private ICallSpecificationFactory _callSpecFactory;
            private Query _query;
            private ICallSpecification _callSpec;
            private RouteAction _result;

            public override void Context()
            {
                _callSpec = mock<ICallSpecification>();
                _call = mock<ICall>();
                _callSpecFactory = mock<ICallSpecificationFactory>();
                _query = mock<Query>();

                _call.stub(x => x.Target()).Return(_target);
                _callSpecFactory.stub(x => x.CreateFrom(_call, MatchArgs.AsSpecifiedInCall)).Return(_callSpec);
            }

            public override void Because()
            {
                _result = sut.Handle(_call);
            }

            [Test]
            public void Add_call_target_and_spec_to_query()
            {
                _query.received(x => x.Add(_target, _callSpec));
            }

            [Test]
            public void Continue_route()
            {
                Assert.That(_result, Is.EqualTo(RouteAction.Continue()));
            }

            public override AddCallToQueryResultHandler CreateSubjectUnderTest()
            {
                return new AddCallToQueryResultHandler(() => _query, _callSpecFactory);
            }
        }
    }
}